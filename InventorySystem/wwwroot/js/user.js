let dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#userTable').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll",
        },
        "columns": [
            { "data": "email", "className":"text-center" },
            { "data": "firstName", "className": "text-center" },
            { "data": "lastName", "className": "text-center" },
            { "data": "phoneNumber", "className": "text-center" },
            { "data": "role", "className": "text-center" },
            {
                "data": { "id": "id", "lockoutEnd": "lockoutEnd" }, "className": "text-center",
                "render": function (data) {
                    let today = new Date().getTime();
                    let lockDate = new Date(data.lockoutEnd).getTime();
                    if (lockDate > today) {
                        return `
                        <div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer;", width:100px>
                                <i class="bi bi-unlock-fill"></i> Unlock
                            </a>
                        </div>
                    `;
                    } else {
                        return `<div class="text-center">
                            <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer;", width:100px>
                                <i class="bi bi-lock-fill"></i> Lock
                            </a>
                        </div>
                     `;
                    }
                },
            }
        ],
    });
}

function LockUnlock(id) {
    $.ajax({
        type: 'POST',
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: 'application/json',
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