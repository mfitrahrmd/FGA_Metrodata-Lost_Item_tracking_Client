(function($) {
  'use strict';
  $(async function() {

    Chart.defaults.global.legend.labels.usePointStyle = true;
    if ($("#found-claim-items-chart").length) {
      Chart.defaults.global.legend.labels.usePointStyle = true;
      var ctx = document.getElementById('found-claim-items-chart').getContext("2d");

      var gradientStrokeViolet = ctx.createLinearGradient(0, 0, 0, 181);
      gradientStrokeViolet.addColorStop(0, 'rgba(218, 140, 255, 1)');
      gradientStrokeViolet.addColorStop(1, 'rgba(154, 85, 255, 1)');
      var gradientLegendViolet = 'linear-gradient(to right, rgba(218, 140, 255, 1), rgba(154, 85, 255, 1))';
      
      var gradientStrokeBlue = ctx.createLinearGradient(0, 0, 0, 360);
      gradientStrokeBlue.addColorStop(0, 'rgba(54, 215, 232, 1)');
      gradientStrokeBlue.addColorStop(1, 'rgba(177, 148, 250, 1)');
      var gradientLegendBlue = 'linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))';

      var gradientStrokeRed = ctx.createLinearGradient(0, 0, 0, 300);
      gradientStrokeRed.addColorStop(0, 'rgba(255, 191, 150, 1)');
      gradientStrokeRed.addColorStop(1, 'rgba(254, 112, 150, 1)');
      var gradientLegendRed = 'linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))';
      
      const foundItems = await $.get({
          url: "/Admin/Items/Found/All",
          headers: {
              "Accept": "application/json"
          }
      });
        
      const claimedItems = await $.get({
          url: "/Admin/Items/Claimed",
          headers: {
              "Accept": "application/json"
          }
      });
      
      const stat = {};
      
      const statFound = foundItems.reduce((acc, curr) => {
          if (acc[new Date(curr.foundAt).toLocaleDateString()]) acc[new Date(curr.foundAt).toLocaleDateString()]++
          else acc[new Date(curr.foundAt).toLocaleDateString()] = 1
          
          return acc
      }, {});
      
      const statClaimed = claimedItems.reduce((acc, curr) => {
          if (acc[new Date(curr.claimedAt).toLocaleDateString()]) acc[new Date(curr.claimedAt).toLocaleDateString()]++
          else acc[new Date(curr.claimedAt).toLocaleDateString()] = 1
          
          return acc
      }, {});
      
      const statLabels = [];

      for (const x in statFound) {
        if (!statClaimed[x]) statClaimed[x] = 0;
      }
      
      for (const x in statClaimed) {
        if (!statFound[x]) statFound[x] = 0;
      }

      for (const x in {...statFound, ...statClaimed}) {
        statLabels.push(new Date(x).toDateString());
      }
      
      const foundCount = Object.values(Object.keys(statFound).sort().reduce((acc, curr) => {
          acc[curr] = statFound[curr];

          return acc;
      }, {}));
      
      const claimedCount = Object.values(Object.keys(statClaimed).sort().reduce((acc, curr) => {
          acc[curr] = statClaimed[curr];

          return acc;
      }, {}));
      
      var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: statLabels,
            datasets: [
              {
                label: "Found",
                borderColor: gradientStrokeViolet,
                backgroundColor: gradientStrokeViolet,
                hoverBackgroundColor: gradientStrokeViolet,
                legendColor: gradientLegendViolet,
                pointRadius: 0,
                borderWidth: 1,
                fill: 'origin',
                data: foundCount
              },
              {
                label: "Claimed",
                borderColor: gradientStrokeRed,
                backgroundColor: gradientStrokeRed,
                hoverBackgroundColor: gradientStrokeRed,
                legendColor: gradientLegendRed,
                pointRadius: 0,
                borderWidth: 1,
                fill: 'origin',
                data: claimedCount
              }
          ]
        },
        options: {
          responsive: true,
          legend: false,
          legendCallback: function(chart) {
            var text = []; 
            text.push('<ul>'); 
            for (var i = 0; i < chart.data.datasets.length; i++) { 
                text.push('<li><span class="legend-dots" style="background:' + 
                           chart.data.datasets[i].legendColor + 
                           '"></span>'); 
                if (chart.data.datasets[i].label) { 
                    text.push(chart.data.datasets[i].label); 
                } 
                text.push('</li>'); 
            } 
            text.push('</ul>'); 
            return text.join('');
          },
          scales: {
              yAxes: [{
                  ticks: {
                      display: false,
                      min: 0,
                      stepSize: 20,
                      max: 10
                  },
                  gridLines: {
                    drawBorder: false,
                    color: 'rgba(235,237,242,1)',
                    zeroLineColor: 'rgba(235,237,242,1)'
                  }
              }],
              xAxes: [{
                  gridLines: {
                    display:false,
                    drawBorder: false,
                    color: 'rgba(0,0,0,1)',
                    zeroLineColor: 'rgba(235,237,242,1)'
                  },
                  ticks: {
                      padding: 20,
                      fontColor: "#9c9fa6",
                      autoSkip: true,
                  },
                  categoryPercentage: 0.5,
                  barPercentage: 0.5
              }]
            }
          },
          elements: {
            point: {
              radius: 0
            }
          }
      })
      $("#found-claim-items-chart-legend").html(myChart.generateLegend());
    }
  });
})(jQuery);
