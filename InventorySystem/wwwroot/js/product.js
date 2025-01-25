let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#prodTable').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll",
        },
        "columns": [
            { "data": "serialNumber", "className": "text-center", "width": "15%" },
            { "data": "prodDescription", "className": "text-center", "width": "14%" },
            { "data": "category.catName", "className": "text-center", "width": "15%" },
            { "data": "brand.brandName", "className": "text-center", "width": "15%" },
            {
                "data": "price", "className": "text-center",
                "render": function (data) {
                    var d = data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
                    return d; 
                }, "width":"15%"
            },
            {
                "data": "status", "className": "text-center",
                "render": function (data) {
                    if (data == true) {
                        return "Active";
                    } else {
                        return "Inactive";
                    }
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer;">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Product/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer;">
                                <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                    `;
                }, "width": "15%"
            }
        ],
    });
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}