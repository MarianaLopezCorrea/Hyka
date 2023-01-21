window.onload = function setCookiesOptions() {
    if (!localStorage.getItem('cookies_options')) {
        $("#cookies_options").modal('show');
        document.getElementById('decline').addEventListener('click', () => {
            localStorage.setItem('cookies_options', "decline all");
            $("#cookies_options").modal('hide');
        });
        document.getElementById('accept').addEventListener('click', () => {
            localStorage.setItem('cookies_options', "accept all");
            $("#cookies_options").modal('hide');
        });
    }
}







