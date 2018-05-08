<script>
    function button_click(objTextBox,objBtnID)
    {
        if(window.event.keyCode==13)
        {
            document.getElementById(objBtnID).focus();
            document.getElementById(objBtnID).click();
        }
    }
</script>