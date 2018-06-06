var menuOpen = false;
function menuFunction(el) {
    //alert("menu")
    var test = el.parentElement;
    //var test = document.getElementById('@ViewBag.grafiekButtons');
    var menuIcons = test.children[0];
    var btnMenu = test.children[1];
    var iconMenu = btnMenu.children[0];
    if (menuOpen === false) {
        iconMenu.style.transform = "rotate(90deg)";
        btnMenu.setAttribute("class", "btn btn-secondary");
        menuIcons.style.visibility = 'visible';
        menuOpen = true;
    } else {
        iconMenu.style.transform = "none";
        btnMenu.setAttribute("class", "btn btn-default");
        menuIcons.style.visibility = "hidden";
        menuOpen = false;
    }
}
// Verplaatsen naar dashboard
function iconDashboardFunction(el) {
    //alert("iconDash " + el);
    var modal = el.parentElement.parentElement;
    //alert(modal.getAttribute("id"));
    var f = {};
    f.url = '@Url.Action("getZonesJson", "Dashboard")';
    f.type = "POST";
    f.dataType = "json";
    f.contentType = "application/json";
    f.success = function (response) {
        alert("succes json");
        setZoneLijstFunction(response, modal);
    };
    //f.error = function (response) {
    //    alert("failed");
    //};
    $.ajax(f);
}
function setZoneLijstFunction(response, modal) {
    //alert("zones " + modal);
    var grafiek = modal.id;
    var grafiekindex = '';
    for (i = 0; i < grafiek.length; i++) {
        if (isNaN(grafiek[i]) === false) {
            grafiekindex = grafiekindex + grafiek[i];
        }
    }
    //alert("grafindex = " + grafiekindex);
    //alert("setLijst");
    var select = document.getElementById("zones");
    select.innerHTML = "";
    for (i = 0; i < response.length; i++) {
        alert("setlijst loop " + response);
        var option = document.createElement("option");
        var a = document.createElement("a");
        var link = "filterElement(\"" + response[i] + "\")";
        option.innerHTML = response[i];
        option.setAttribute("value", response[i]);
        select.appendChild(option);
    }
}
function saveGrafiek(el) {
    var parent = el.parentElement.parentElement.parentElement.parentElement;
    var grafiek = parent.id;
    for (i = 0; i < grafiek.length; i++) {
        if (isNaN(grafiek[i]) === false) {
            grafiekindex = grafiekindex + grafiek[i];
        }
    }
    var select = parent.children[0].children[0].children[1].children[1];
    alert(select.id);
    var zone = select.options[select.selectedIndex].value;
    var f = {};
    f.url = '@Url.Action("dashboardGrafiek", "Dashboard")';
    f.type = "POST";
    f.dataType = "json";
    f.data = JSON.stringify({ zone: zone, grafiekIndex: grafiekindex });
    f.contentType = "application/json";
    f.success = function (response) {
        alert("grafiek opgeslagen");
    };
    f.error = function (response) {
        alert("failed");
    };
    $.ajax(f);
}

// allert grafiek
function alertSelect(el) {
    var option = el.options[el.selectedIndex].value;
    var radioform = el.parentElement.children[3];
    var posRadio = radioform.children[2].children[0];
    var negRadio = radioform.children[3].children[0];
    if (option === 'trend') {

        posRadio.style.visibility = "hidden";
        negRadio.style.visibility = "hidden";
    } else {

        posRadio.style.visibility = "visible";
        negRadio.style.visibility = "visible";
    }
}
function saveAlert(el) {
    var parent = el.parentElement.parentElement.parentElement.parentElement;

    var grafiek = parent.id;
    for (i = 0; i < grafiek.length; i++) {
        if (isNaN(grafiek[i]) === false) {
            grafiekindex = grafiekindex + grafiek[i];
        }
    }
    var modalbody = parent.children[0].children[0].children[1];
    var alertselect = modalbody.children[1];
    var radioform = modalbody.children[3];
    var meldform = modalbody.children[6];
    var stijgtRadio = radioform.children[0].children[0].children[0];
    var stijgtPercent = radioform.children[0].children[0].children[1];
    var daaltRadio = radioform.children[1].children[0].children[0];
    var daaltPercent = radioform.children[1].children[0].children[1];
    var posRadio = radioform.children[2].children[0].children[0];
    var posPercent = radioform.children[2].children[0].children[1];
    var negRadio = radioform.children[3].children[0].children[0];
    var negPercent = radioform.children[3].children[0].children[1];
    var browser = meldform.children[0].children[0].children[0].checked;
    var mail = meldform.children[1].children[0].children[0].checked;
    var app = meldform.children[2].children[0].children[0].checked;


    var option = alertselect.options[alertselect.selectedIndex].value;
    var selectedRadio = "";
    var percentage = 0;

    if (stijgtRadio.checked === true) {
        percentage = stijgtPercent.value;
        selectedRadio = "stijgt";
    } else if (daaltRadio.checked === true) {
        percentage = daaltPercent.value;
        selectedRadio = "daalt";
    } else if (posRadio.checked === true) {
        percentage = posPercent.value;
        selectedRadio = "pos";
    } else if (negRadio.checked === true) {
        percentage = negPercent.value;
        selectedRadio = "neg";
    } else {
        alert("foutje");
    };
    //tot hier in orde
    var f = {};
    f.url = '@Url.Action("addAlert", "Dashboard")';
    f.type = "POST";
    f.dataType = "json";
    f.data = JSON.stringify({ id: grafiekindex, percentage: percentage, radio: selectedRadio, soort: option, browser: browser, mail: mail, app: app });
    f.contentType = "application/json";
    f.success = function (response) {
        alert("alert opgeslagen");
    };
    f.error = function (response) {
        alert("failed");
    };
    $.ajax(f);
}

//verwijder grafiek
function deleteGrafiek(el) {
    var parent = el.parentElement.parentElement.parentElement.parentElement;
    var grafiek = parent.id;
    for (i = 0; i < grafiek.length; i++) {
        if (isNaN(grafiek[i]) === false) {
            grafiekindex = grafiekindex + grafiek[i];
        }
    };
    var f = {};
    f.url = '@Url.Action("deleteGrafiek", "Dashboard")';
    f.type = "POST";
    f.dataType = "json";
    f.data = JSON.stringify({ grafiekIndex: grafiekindex });
    f.contentType = "application/json";
    f.success = function (response) {
        alert("grafiek verwijderd");
    };
    f.error = function (response) {
        alert("failed");
    };
    $.ajax(f);
}

//edit grafiek
function editGrafiek(el) {
    var parent = el.parentElement.parentElement.parentElement.parentElement;
    var grafiek = parent.id;
    for (i = 0; i < grafiek.length; i++) {
        if (isNaN(grafiek[i]) === false) {
            grafiekindex = grafiekindex + grafiek[i];
        }
    };
    var f = {};
    f.url = '@Url.Action("editGrafiek", "Dashboard")';
    f.type = "POST";
    f.dataType = "json";
    f.data = JSON.stringify({ grafiekIndex: grafiekindex });
    f.contentType = "application/json";
    f.success = function (response) {
        alert("grafiek gewijzigd");
    };
    f.error = function (response) {
        alert("failed");
    };
    $.ajax(f);
}