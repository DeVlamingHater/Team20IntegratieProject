function loadZone(evt, zoneId) {
    // Declare all variables
    var i, tabcontent, tablinks;

    // Get all elements with class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Get all elements with class="tablinks" and remove the class "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Show the current tab, and add an "active" class to the button that opened the tab
    document.getElementById(zoneId).style.display = "block";
    evt.currentTarget.className += " active";
}
function veranderTabNaam(button) {
    button.contentEditable = "true";
}
function saveTabNaam(id, button) {
    var zoneId = id;
    var tekst = button.innerText;
    var f = {};
    f.url = '@Url.Action("saveTabNaam", "Dashboard")';
    f.type = "POST";
    f.dataType = "json";
    f.data = JSON.stringify({ id: zoneId, naam: tekst });
    f.contentType = "application/json";
    f.success = function (response) {
    };
    f.error = function (response) {
    };
    $.ajax(f);
}

