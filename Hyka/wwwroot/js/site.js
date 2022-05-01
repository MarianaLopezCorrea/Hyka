
window.onload = async function getUsersTable() {
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
            for (i = 0; i < users.length; i++) {
                table += `
                <tbody>
                    <tr class="table-light">
                        <td>${users[i].id}</td>
                        <td>${users[i].fullName}</td>
                    </tr>
                </tbody>`
            }
            table += '</table>';
            $("#users-table").append(table);
        },
    });
}


async function getImage(data) {
    const url = 'https://api.qrserver.com/v1/create-qr-code/?data=' + data
    let image = document.getElementById('image')
    await fetch(url)
        .then(response => { return response.url })
        .then(data => {
            image.src = data
        })
}
