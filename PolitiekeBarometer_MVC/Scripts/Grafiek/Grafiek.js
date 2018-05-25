function laadGrafiek(id) {

    var chart = document.getElementById(id);
    var entries = document.getElementById(id + " entry").innerHTML;
    entries = entries.trim('/');
    entries = JSON.parse(entries);
    var label = document.getElementById(id + " label").innerHTML;
    label = label.trim('/');
    label = JSON.parse(label);

    var test = document.getElementById("test");
    test.innerHTML = label + entries;
    console.log(label + entries);
    var graph = new Chart(chart, {
        type: 'line',
        data: {
            labels: label,
            datasets: [{
                label: "Sessions",
                lineTension: 0.3,
                backgroundColor: "rgba(2,117,216,0.2)",
                borderColor: "rgba(2,117,216,1)",
                pointRadius: 5,
                pointBackgroundColor: "rgba(2,117,216,1)",
                pointBorderColor: "rgba(255,255,255,0.8)",
                pointHoverRadius: 5,
                pointHoverBackgroundColor: "rgba(2,117,216,1)",
                pointHitRadius: 20,
                pointBorderWidth: 2,
                data: entries
            }]
        },
        options:
            {
                scales: {
                    xAxes: [{
                        time: {
                            unit: 'date'
                        },
                        gridLines: {
                            display: false
                        },
                        ticks: {
                            maxTicksLimit: 7
                        }
                    }],
                    yAxes: [{
                        ticks: {
                            min: 0,
                            max: 120,
                            maxTicksLimit: 5
                        },
                        gridLines: {
                            color: "rgba(0, 0, 0, .125)",
                        }
                    }],
                },
                legend: {
                    display: false
                }
            }
    });
}