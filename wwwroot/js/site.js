function getUsersTable() {
    $.ajax({
        url: 'https://localhost:44397/api/person/get',
        type: 'GET',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (users) {
            $("#users-table").empty();
            let table = '<table>';
            for (i = 0; i < users.length; i++) {
                console.log(users);
                table += '<tr>';
                table += '<td>' + users[i].id + '</td>';
                table += '<td>' + users[i].fullName+ '</td>';
                // table += '<td>' +     users[i].client.idClient + '</td>';
                // table += '<td>' + users[i].client.name + '</td>';
                // table += '<td>' + users[i].client.email + '</td>';
                // table += '<td>' + users[i].score + '</td>';
                 table += '<td><button onclick="editarRegistro(' + users[i].idReservation + ' )">Editar</button>';
                 table += '<td><button onclick="eliminarRegistro(' + users[i].idReservation + ' )">Borrar</button>';
                 table += '</tr>';

            }
            table += '</table>';
            $("#users-table").append(table);
        },
    });
}
