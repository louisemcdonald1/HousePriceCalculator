
Chart.defaults.global.responsive = true;
Chart.defaults.global.animation = true;
Chart.defaults.global.animationSteps = true;

var ctx = document.getElementById("myChart").getContext("2d");

    var data = {
        labels: graphLabels.slice(0),
        datasets: [
            {
                label: "My First dataset",
                fillColor: "rgba(220,220,220,0.2)",
                strokeColor: "rgba(220,220,220,1)",
                pointColor: "rgba(220,220,220,1)",
                pointStrokeColor: "#fff",
                pointHighlightFill: "#fff",
                pointHighlightStroke: "rgba(220,220,220,1)",
                data: graphNumbers
            }
        ]
    };//end data definition

    var myLineChart = new Chart(ctx).Line(data, { legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].strokeColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>" });
