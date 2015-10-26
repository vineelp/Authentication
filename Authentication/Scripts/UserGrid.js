$(function () {
    $("#grid").jqGrid({
        url: "/Account/GetUsersList",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['UserId', 'First Name', 'Last Name', 'UserName', 'EmailAddress', 'Password'],
        colModel: [
            { key: false, name: 'UserID', key: true, hidden: true, index: 'UserID', editable: true },
            { key: false, name: 'FirstName', index: 'FirstName', editable: true },
            { key: false, name: 'LastName', index: 'LastName', editable: true },
            { key: false, name: 'UserName', index: 'UserName', editable: false},
            { key: false, name: 'EmailAddress', index: 'EmailAddress', editable: true },
            { key: false, name: 'Password', index: 'Password', editable: true}],
        pager: jQuery('#pager'),
        rowNum: 5,
        rowList: [2, 5, 10, 15],
        height: '100%',
        viewrecords: true,
        caption: 'Users List',
        emptyrecords: 'No records to display',
        jsonReader: {
            root: "rows",
            page: "page",
            total: "total",
            records: "records",
            repeatitems: false,
            Id: "0"
        },
        autowidth: true,
        multiselect: false
    }).navGrid('#pager', { edit: true, add: false, del: true, search: false, refresh: true },
        {
            // edit options
            zIndex: 100,
            url: '/Account/EditUser',
            closeOnEscape: true,
            closeAfterEdit: true,
            recreateForm: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // add options
            zIndex: 100,
            url: "/TodoList/Create",
            closeOnEscape: true,
            closeAfterAdd: true,
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        },
        {
            // delete options
            zIndex: 100,
            url: "/Account/DeleteUser",
            closeOnEscape: true,
            closeAfterDelete: true,
            recreateForm: true,
            msg: "Are you sure you want to delete?",
            afterComplete: function (response) {
                if (response.responseText) {
                    alert(response.responseText);
                }
            }
        });
    $('#filterButton').click(function (event) {
        event.preventDefault();
        filterGrid();
    });
    $('#reset').click(function (event) {
        $('.filterItem').val('');
        filterGrid();
    });
    $('.filterItem').addClass('form-control');
});

function filterGrid() {
    var postDataValues = $("#grid").jqGrid('getGridParam', 'postData');

    // grab all the filter ids and values and add to the post object
    $('.filterItem').each(function (index, item) {
        postDataValues[$(item).attr('id')] = $(item).val();
    });

    $('#grid').jqGrid().setGridParam({ postData: postDataValues, page: 1 }).trigger('reloadGrid');
}