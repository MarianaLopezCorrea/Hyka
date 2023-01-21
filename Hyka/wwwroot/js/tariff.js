function getDataFromTariff(encondeDto) {
    try {
        const object = JSON.parse(atob(encondeDto));
        Object.entries(object).forEach(([key, value]) => {
            const element = document.getElementById(key);
            element.value = value;
        });
        $('#editTariff').modal('show');
    } catch (error) {
        return;
    }
}