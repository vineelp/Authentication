$(function () {
    $("#grid").jqGrid({
        url: "/Account/GetUsersList",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['UserId', 'First Name', 'Last Name', 'UserName', 'EmailAddress', 'Password', 'Active'],
        colModel: [
            { key: false, name: 'UserID', key: true, hidden: true, index: 'UserID', editable: true },
            { key: false, name: 'FirstName', index: 'FirstName', editable: true },
            { key: false, name: 'LastName', index: 'LastName', editable: true },
            { key: false, name: 'UserName', index: 'UserName', editable: false},
            { key: false, name: 'EmailAddress', index: 'EmailAddress', editable: true },
            { key: false, name: 'Password', index: 'Password', editable: true },
            { key: false, name: 'Active', index: 'Active', editable: true
                , formatoptions: { disabled: true }, edittype: "checkbox"
                , editoptions: { value: "Y:N"}
            }],
        pager: jQuery('#pager'),
        rowNum: 10,
        rowList: [2, 5, 10, 15],
        height: '100%',
        viewrecords: true,
        caption: 'User Administration',
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
    }).navGrid('#pager', { edit: false, add: false, del: false, search: false, refresh: false },
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
    $("#grid").jqGrid('inlineNav', '#pager',
    {
        edit: true,
        editicon: "ui-icon-pencil",
        add: false,
        addicon: "ui-icon-plus",
        save: true,
        saveicon: "ui-icon-disk",
        cancel: true,
        cancelicon: "ui-icon-cancel",

        editParams: {
            keys: false,
            oneditfunc: null,
            successfunc: function (val) {
                if (val.responseText != "") {
                    alert(val.responseText);
                    $(this).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                }
            },
            url: '/Account/EditUser',
            extraparam: {
                EmpId: function () {
                    var sel_id = $('#jQGridDemo').jqGrid('getGridParam', 'selrow');
                    var value = $('#jQGridDemo').jqGrid('getCell', sel_id, '_id');
                    return value;
                }
            },
            aftersavefunc: null,
            errorfunc: null,
            afterrestorefunc: null,
            restoreAfterError: true,
            mtype: "POST"
        }
    });
});


function filterGrid() {
    var postDataValues = $("#grid").jqGrid('getGridParam', 'postData');

    // grab all the filter ids and values and add to the post object
    $('.filterItem').each(function (index, item) {
        postDataValues[$(item).attr('id')] = $(item).val();
    });

    $('#grid').jqGrid().setGridParam({ postData: postDataValues, page: 1 }).trigger('reloadGrid');
}