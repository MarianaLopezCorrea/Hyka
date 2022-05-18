
async function getUsersTable() {
    await $.ajax({
        url: 'https://localhost:8080/api/person/get',
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (users) {
            $("#users-table").empty();
            let table = `
            <table class="table table-bordered table-striped">
                <thead>
                    <tr class="table-primary" style="text-align: center;">
                        <th>Cedula</th>
                        <th>Nombre</th>
                    </tr>
                </thead>`;
            for (const user of users) {
                table += `
                <tbody>
                    <tr class="table-light">
                        <td>${user.id}</td>
                        <td>${user.fullName}</td>
                    </tr>
                </tbody>`
            }
            table += '</table>';
            $("#users-table").append(table);
        },
    });
}


async function getImage(text) {
    const url = 'https://api.qrserver.com/v1/create-qr-code/?data=' + text
    let image = document.getElementById('image')
    await fetch(url)
        .then(response => { return response.url })
        .then(data => {
            image.src = data
        })
}

function getFlagEmoji(countryCode) {
    const codePoints = countryCode
        .toUpperCase()
        .split('')
        .map(char => 127397 + char.charCodeAt());
    return String.fromCodePoint(...codePoints);
}

async function getAllContries() {
    const url = 'https://restcountries.com/v2/all'
    let selector = document.querySelector("#countries")
    await fetch(url)
        .then(response => { return response.json() })
        .then(countries => {
            let data = '';
            for (const country of countries) {
                let emoji = getFlagEmoji(country.alpha3Code);
                let name = country.translations.es
                data += `<option value="${name.toUpperCase()}"> ${name} ${emoji}</option>`
            }
            selector.innerHTML = data
        })
}

