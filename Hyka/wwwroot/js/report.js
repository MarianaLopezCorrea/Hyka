function generateReportTable(data) {
    if (data == '') return;
    try {
        const objects = JSON.parse(atob(data));
        const columns = Object.keys(objects[0]).map(key => {
            return {
                data: key,
            }
        });
        Object.keys(objects[0]).forEach(key => {
            $('#rows').append(`<th>${key}</th>`);
        });
        $(document).ready(function () {
            let table = $('#report_table').DataTable({
                dom: 'Bfrtip',
                data: objects,
                columns: columns,
                columnDefs: [
                    {
                        targets: '_all',
                        className: 'dt-center'
                    },
                    {
                        target: columns.length - 1,
                        render: DataTable.render.date('yyyy-MM-dd'),
                    }
                ],
                buttons: [
                    {
                        extend: 'colvis',
                        collectionLayout: 'fixed columns',
                        text: '<i class="bi bi-eye-fill"></i>',
                        titleAttr: 'Visibilidad'
                    },
                    {
                        extend: 'collection',
                        text: '<i class="bi bi-folder-fill"></i>',
                        buttons: [
                            {
                                extend: 'excel',
                                excelStyles: {
                                    "template": [
                                        "blue_medium",
                                        "header_green",
                                        "title_medium"
                                    ]
                                }
                            },
                            'pdf',
                            'csv'
                        ],
                        titleAttr: 'Descargar',
                    },
                    {
                        extend: 'copy',
                        text: '<i class="bi bi-clipboard2-fill"></i>',
                        titleAttr: 'Copiar',
                    },

                    {
                        extend: 'print',
                        text: '<i class="bi bi-printer-fill"></i>',
                        titleAttr: 'Imprimir',
                    }
                ],
                language: {
                    url: '//cdn.datatables.net/plug-ins/1.12.1/i18n/es-ES.json',
                },
                initComplete: function () {
                    $("#report").css("opacity", "0");
                    $('#load').remove();
                    $("#report").css("visibility", "visible");
                    $("#report").css("opacity", "1");
                    $("#collapseReports").collapse("hide");
                },
            });
        });
    } catch (error) {
        console.log(error);
    }
}