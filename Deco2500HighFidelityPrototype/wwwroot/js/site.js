// Write your JavaScript code.
$(function () {

    //first check which page we're on!
    if ($("#dietGraph").length) {
        GetDietGraphData();
    }
});

function GetDietGraphData() {
    var id = $("#dunnoYet").val(); // whatever I call the id for the thingy
    var sendRequestTo = window.location.origin + POST_DIETGRAPH_URL + id;
    $.ajax({
        type: "POST",
        url: sendRequestTo,
        success: MakeDietChart,
        error: AlertError
    });
}
var POST_DIETGRAPH_URL = "/Diet/GetDietGraphData/id?";

function MakeDietChart(data) {
    // data will be a list sent from the server
    var ctx = document.getElementById("dietGraph").getContext('2d');
    // do chart stuff - this is example code from chart.js docs
    // modify for our chart
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
            datasets: [{
                label: '# of Votes',
                data: [12, 19, 3, 5, 2, 3],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
}
function AlertError(xhr, status, error) {
    //do nothing for now
}

