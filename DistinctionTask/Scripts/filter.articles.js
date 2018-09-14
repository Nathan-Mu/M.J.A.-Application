$(function () {
    $('#data').DataTable({
        "order": [[1, "asc"]],
        "dom": "lrtip",
        "columnDefs": [{ "targets": [5], "searchable": false }]
    });
});