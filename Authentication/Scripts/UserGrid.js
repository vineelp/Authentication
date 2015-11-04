$(function () {
    $("#grid").jqGrid({
        url: "/Account/GetUsersList",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Action', 'UserName', 'UserId', 'First Name', 'Last Name', 'EmailAddress', 'Password', 'Active'],
        colModel: [
                { name: 'documentName', index: 'documentName', width: 35, editable: false,
                    //add the following in one of the colModel config
                    formatter: 'actions',
                    formatoptions: {
                        keys: true,
                        delbutton: false,
                        editformbutton: true,
                        editOptions: {
                            //closeAfterEdit: true,
                            url: '/Account/EditUser',
                            afterComplete: function (response) {
                                if (response.responseText) {
                                    alert(response.responseText);
                                    $(this).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                                    if (response.responseText === "Saved Successfully")
                                        $('#cData').trigger('click');
                                }
                            }
                        }
                    }
                },
            { key: false, name: 'UserName', index: 'UserName', editable: false},
            { key: false, name: 'UserID', key: true, hidden: true, index: 'UserID', editable: true },
            { key: false, name: 'FirstName', index: 'FirstName', editable: true },
            { key: false, name: 'LastName', index: 'LastName', editable: true },
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