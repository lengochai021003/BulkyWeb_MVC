﻿
var dataTable;
$(function () {
    loadDataTable();
});
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "pageLength": 5,
        "columns": [
            { data: 'productID', "width": "3%" },
            { data: 'titleProduct', "width": "15%" },
            { data: 'author', "width": "15%" },
            { data: 'isbn', "width": "15%" },
            { data: 'listPrice', "width": "3%" },
            { data: 'category.categoryName', "width": "15%" },
            {
                data: 'productID',
                "render": function (data) {
                    return `<div class = "w-75 btn-group" role="group"> 
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary "> <i class="bi bi-pencil-square"></i></a>
                        <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger "> <i class="bi bi-trash-fill"></i></a>
                    </div>`
                },
                "width": "15%" 
            }
            
        ]
    });
};

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}    