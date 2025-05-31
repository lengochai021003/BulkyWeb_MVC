var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "pageLength": 5,
        "columns": [
            { data: 'name', "width": "15%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'company.name', "width": "3%" },
            { data: 'role', "width": "15%" },
            {
                data: {id: "id", lockEnd: "lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                            <div class="text-center">
                                <a onClick="LockUnlock('${data.id}')" class="btn btn-success text-white" style="cursor:pointer; width: 100px;">
                                    <i class="bi bi-unlock-fill"></i> Unlock
                                </a>
                                <a href="/Admin/User/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width: 200px;">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    } else {
                        return `
                            <div class="text-center">
                                <a onClick="LockUnlock('${data.id}')" class="btn btn-success text-white" style="cursor:pointer; width: 100px;">
                                    <i class="bi bi-lock-fill"></i> Lock
                                </a>
                                <a href="/Admin/User/RoleManagment?userId=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; width: 200px;">
                                    <i class="bi bi-pencil-square"></i> Permission
                                </a>
                            </div>
                        `;
                    }
                },
                "width": "30%"
            }
        ]
    });
}

function LockUnlock(id) {
    
        $.ajax({
            type: "POST",
            url: '/Admin/User/LockUnlock',
            data: JSON.stringify(id),
            contentType: "application/json",
            success: function (data) {
                if (data.success) {
                    toastr.success(data.message);
                    dataTable.ajax.reload(); // Reload table để cập nhật trạng thái
                } else {
                    toastr.error(data.message || 'Error occurred');
                }
            },
            error: function (xhr, status, error) {
                console.error('AJAX Error:', error);
                toastr.error('Something went wrong!');
            }
        });
}