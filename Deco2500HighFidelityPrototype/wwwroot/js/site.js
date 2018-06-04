// Write your JavaScript code.
$(function () {
    var y = $(window).scrollTop();  //your current y position on the page
    $(window).scrollTop(y + 150);
    var boundWeightEvent = false;
    var numIngredients = 0;
    //first check which page we're on!
    if ($("#dietGraph").length) {
        GetDietGraphData();
    }
    if ($("#fitnessGraph").length) {
        GetFitnessGraphData();
    }
    if ($("#welcomeGraph").length) {
        MakeWelcomeFitnessChart();
    }
    if ($("#welcomeGraph2").length) {
        MakeWelcomeDietChart();
    }
    if ($(".mealChooserButton").length) {
        $(".mealChooserButton").on("click", function () {
            SendMealChoice($(this).next().val())
        });
    }
    $("#noIngredientSelected").hide();

    $("#createMealButton").on('click', function () {
        if (numIngredients == 0) {
            $("#noIngredientSelected").show().delay(5000).fadeOut();
        }
        else {
            //gather ingredients from places!
            var ingredients = [];
            var name = "New Meal"
            if ($("#mealName").val() != "") {
                name = $("#mealName").val();
            }
            $("#hiddenIngredientsList").children().each(function (i) {
                ingredients[i] = $(this).val();
            });
            $.ajax({
                type: "POST",
                url: window.location.origin + POST_CREATEMEAL_URL,
                dataType: "json",
                data: {
                    ingredients: ingredients,
                    name: name
                },
                success: function () {
                    window.location.href = window.location.origin + POST_MEALDETAILS_URL;
                },
                error: AlertError
            })
        }
    });

    if ($('#ingredientAutocomplete').length) {
        $('#ingredientAutocomplete').autocomplete({
            source: (request, response) => {
                $.ajax({
                    type: "POST",
                    url: window.location.origin + GET_ALLINGREDIENTS_URL,
                    data: { Message: request.term },
                    dataType: "json",
                    success: (data) => {
                        response(data);
                    },
                    error: AlertError
                });

            },
            minLength: 2,
            delay: 100,
            select: (event, ui) => {
                event.preventDefault();
                var match = false;
                var currentChildren = $("#hiddenIngredientsList").children();
                for (var i = 0; i < $(currentChildren).length; i++) {
                    if ($(currentChildren[i]).val().includes(ui.item.value))
                        match = true;
                }
                if (!match) {
                    $("#currentIngredientList")
                        .append('<li class="list-group-item bigFont ingItem"> <span>'
                        + ui.item.label +
                        '</span><span> <span>weight(g):</span> <input type="number" id="ingInputId_'
                        + numIngredients +
                        '" class="weightAmount" min="1.0" value="1.0" step="0.01" /><i class="fas fa-ban tomato"></i></span></li>');
                    $("#hiddenIngredientsList")
                        .append('<input type="hidden" id="idIng' + numIngredients + '" value="' + ui.item.value + '_' + '1.0" />');

                    $('.weightAmount').on('change', function () {
                        var id = $(this)[0].id;
                        var index = id.split("_")[1];
                        var oldVal = $("#idIng" + index).val().split("_")[0];
                        $("#idIng" + index).val(oldVal + "_" + $(this).val());
                    });
                    $('.fa-ban').on('click', function () {
                        var index = $(this).prev()[0].id.split("_")[1];
                        var li = $(this).parent().parent();
                        li.remove();
                        $("#idIng" + index).remove();
                        if (numIngredients > 0)
                            numIngredients--;
                    });
                    boundWeightEvent = true;

                    numIngredients++;
                }
                $(this).val(ui.item.label);
                //return false;
            },
            change: function (ev, ui) {
                if (ui.item) {
                    $(this).val('');
                }
            },
            focus: function (event, ui) {
                $(this).val() = ui.item.label;
                // or $('#autocomplete-input').val(ui.item.label);

                // Prevent the default focus behavior.
                event.preventDefault();
                // or return false;
            }
        });
    }
});

function GetDietGraphData() {
    var id = $("#userId").val(); // whatever I call the id for the thingy
    var sendRequestTo = window.location.origin + POST_DIETGRAPH_URL;
    $.ajax({
        type: "POST",
        url: sendRequestTo,
        dataType: "json",
        data: { Id: id },
        success: MakeDietChart,
        error: AlertError
    });
}
function GetFitnessGraphData() {
    var id = $("#userId").val(); // whatever I call the id for the thingy
    var sendRequestTo = window.location.origin + POST_FITNESSGRAPH_URL;
    $.ajax({
        type: "POST",
        url: sendRequestTo,
        dataType: "json",
        data: { Id: id },
        success: MakeFitnessChart,
        error: AlertError
    });
}
function SendMealChoice(message) {
    var sendRequestTo = window.location.origin + POST_CHOOSEMEAL_URL;
    var mealDetails = window.location.origin + POST_MEALDETAILS_URL;
    $.ajax({
        type: "POST",
        url: sendRequestTo,
        dataType: "json",
        data: { Message: message },
        success: () =>
            window.location.replace(mealDetails)
        ,
        error: AlertError
    });
}
var POST_DIETGRAPH_URL = "/Diet/GetDietGraphData/";
var POST_FITNESSGRAPH_URL = "/Fitness/GetFitnessGraphData/";
var POST_CHOOSEMEAL_URL = "/Diet/ChooseMeal/";
var POST_MEALDETAILS_URL = "/Diet/MealDetails/";
var GET_ALLINGREDIENTS_URL = "/Diet/GetAllIngredients/";
var POST_CREATEMEAL_URL = "/Diet/CreateMeal/";
function MakeDietChart(data) {
    // data will be a list sent from the server
    var ctx = document.getElementById("dietGraph").getContext('2d');
    // do chart stuff - this is example code from chart.js docs
    // modify for our chart
    var lemmeSee = data;
    var labels = [];
    var displayData = [];
    var dates = [];

    for (var i = 0; i < data.length; i++) {
        labels[i] = "";
        for (var j = 0; j < data[i].ingredients.length; j++) {
            //concatemate strings
            labels[i] += data[i].ingredients[j] + " ";
        }
        labels[i] = labels[i].substring(0, labels[i].length - 1);
        displayData[i] = data[i].calories;
    }

    data.forEach(meal => dates.push(moment(meal.date)));
    var myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: dates,
            datasets: [{
                label: 'Calories',
                data: displayData,
                backgroundColor: [
                    '#47d179',
                    '#47d179',
                    '#47d179',
                    '#47d179',
                    '#47d179',
                    '#47d179',
                    '#47d179'
                ],
                borderColor: [
                    '#28a745',
                    '#28a745',
                    '#28a745',
                    '#28a745',
                    '#28a745',
                    '#28a745',
                    '#28a745'
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
                }],
                xAxes: [{
                    type: 'time',
                    time: {
                        displayFormats: {
                            day: 'MMM D'
                        }
                    }
                }]
            },
            title: {
                display: true,
                text: "Last 5 Meals",
                fontSize: 16,
                fontFamily: "Segoe UI"
            },
            legend: {
                labels: {
                    fontSize: 16,
                    fontFamily: "Segoe UI"
                }
            }
        }
    });
}
function MakeFitnessChart(data) {
    // data will be a list sent from the server
    var ctx = document.getElementById("fitnessGraph").getContext('2d');
    // do chart stuff - this is example code from chart.js docs
    // modify for our chart
    var lemmeSee = data;
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
function MakeWelcomeFitnessChart() {
    // data will be a list sent from the server
    var ctx = document.getElementById("welcomeGraph").getContext('2d');
    // do chart stuff - this is example code from chart.js docs
    // modify for our chart
    var percent = $("#percentFit").val() * 100;
    var goal = 100 - percent;
    var myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Time Spent Exercising", "Daily Fitness Goal Remaining"],
            datasets: [{
                data: [percent, goal],
                backgroundColor: [
                    '#FAA43A',
                    '#5DA5DA'
                ]
            }]
        },
        options: {
            animation: {
                animateRotate: true
            },
            rotation: Math.PI * 3 / 2,
            legend: {
                labels: {
                    fontSize: 16,
                    fontFamily: "Segoe UI"
                }
            }
        }
    });
}
function MakeWelcomeDietChart() {
    // data will be a list sent from the server
    var ctx = document.getElementById("welcomeGraph2").getContext('2d');
    // do chart stuff - this is example code from chart.js docs
    // modify for our chart
    var percent = $("#percentDiet").val() * 100;
    var goal = 100 - percent;
    var myChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Calories Consumed Today", "Daily Diet Goal Remaining"],
            datasets: [{
                data: [percent, goal],
                backgroundColor: [
                    '#4858e2',
                    "#c92031"
                ]
            }]
        },
        options: {
            animation: {
                animateRotate: true
            },
            rotation: Math.PI * 3 / 2,
            legend: {
                labels: {
                    fontSize: 16,
                    fontFamily: "Segoe UI"
                }
            }
        }
    });
}
function AlertError(xhr, status, error) {
    //do nothing for now
}