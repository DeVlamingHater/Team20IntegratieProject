function laadGrafiek(id) {
    var entries = document.getElementById(id + " entry").innerHTML;
    entries = entries.trim('/');
    entries = JSON.parse(entries);
    var label = document.getElementById(id + " label").innerHTML;
    label = label.trim('/');
    label = JSON.parse(label);
    var grafiekType = document.getElementById(id + " type").innerHTML;

    switch (grafiekType) {
        case "BAR":
            barGrafiek(label, entries, id);
            break;
        case "LINE":
            lineGrafiek(label, entries, id);
            break;
        case "PIE":
            pieGrafiek(label, entries, id);
            break;
        default:
    }

}
function barGrafiek(label, entries, id) {
    var ctx = document.getElementById(id + " grafiek");
    var myChart = new Chart(ctx, {
        type: "bar",
        data: {
            labels: label,
            datasets: [{
                label: "Trending",
                backgroundColor: "rgba(2,117,216,1)",
                borderColor: "rgba(2,117,216,1)",
                data: entries,
            }],
        },
        options: {
            maintainAspectRatio: true,
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        maxTicksLimit: 6
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero:true,
                        maxTicksLimit: 10
                    },
                    gridLines: {
                        display: true
                    }
                }],
            },
            legend: {
                display: true
            }
        }
    });
}

function lineGrafiek(label, entries,id) {
    var ctx = document.getElementById(id +" grafiek");

    console.log(label + entries);
    var graph = new Chart(ctx, {
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

function pieGrafiek(label, entries, id) {
    var ctx = document.getElementById(id + " grafiek");

    var myChart = new Chart(ctx, {
        type: "doughnut",
        data: {
            labels: label,
            datasets: [{
                data: entries,
                backgroundColor: ['#FF6633', '#FFB399', '#FF33FF', '#FFFF99', '#00B3E6',
                    '#E6B333', '#3366E6', '#999966', '#99FF99', '#B34D4D',
                    '#80B300', '#809900', '#E6B3B3', '#6680B3', '#66991A',
                    '#FF99E6', '#CCFF1A', '#FF1A66', '#E6331A', '#33FFCC',
                    '#66994D', '#B366CC', '#4D8000', '#B33300', '#CC80CC',
                    '#66664D', '#991AFF', '#E666FF', '#4DB3FF', '#1AB399',
                    '#E666B3', '#33991A', '#CC9999', '#B3B31A', '#00E680',
                    '#4D8066', '#809980', '#E6FF80', '#1AFF33', '#999933',
                    '#FF3380', '#CCCC00', '#66E64D', '#4D80CC', '#9900B3',
                    '#E64D66', '#4DB380', '#FF4D4D', '#99E6E6', '#6666FF'],
            }],
        },
    });
}