$('#tblData').DataTable({
    ajax: '/Product/GetAll',
    columns: [
        { data: 'title', width: '20%' },
        { data: 'isbn', width: '15%' },
        { data: 'price', width: '10%', "render": function (data) { return '$' + data.toFixed(2); } },
        { data: 'author', width: '15' },
        {
            data: 'category.categoryName', width: '15', "return": function (data)
            { return '<span class= "badge bg-secondary">' + data + '</span>'; }
        },
        {
            data: 'id', width: '25%', "render": function (data)
            {
                return `<div class="d-flex gap-2 justify-content-end">
                       <a href="/product/Upsert?id=${data}" class="btn btn-sm btn-outline-success"><i class="bi-bi-pencil-square"></i>Edit</a>
                       <a href="/product/Delete?id=${data}" class="btn btn-sm btn-outline-danger"><i class="bi-bi-trash"></i> Delete </a>
                       </div > `;
            }
        }
      
    ]
});