$(function () {
    var dbEditWidth = 30;
    var dbWidth = 400;
    var editOptions = {
        url: '/Account/EditUser',
        width: dbWidth,
        top: Math.max(0, ($(window).height() / 3)),
        left: Math.max(0, ($(window).width() / 3)),
        closeOnEscape: true,
        beforeShowForm: function (formid) {
            $('#editmodgrid  input').keyup(function (event) {
                if (event.keyCode == 13) {
                    $("#sData").click();
                }
            });
        },
        afterComplete: function (response) {
            if (response.responseText) {
                $(this).jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                if (response.responseText.indexOf("Saved") > -1) {
                    $('#cData').trigger('click');
                    alert(response.responseText);
                } else {
                    $("#FormError").show();
                    $("#FormError>td").html(response.responseText);
                }
            }
        }
    };
    $("#grid").jqGrid({
        url: "/Account/GetUsersList",
        datatype: 'json',
        mtype: 'Get',
        colNames: ['Actions', 'UserName', 'UserId', 'First Name', 'Last Name', 'EmailAddress', 'Password', 'Active'],
        colModel: [
                {
                    name: 'Actions', index: 'Actions', width: 35, editable: false,
                    formatter: 'actions',
                    formatoptions: {
                        keys: true,
                        delbutton: false,
                        editformbutton: true,
                        editOptions: editOptions
                    }
                },
            { key: false, name: 'UserName', index: 'UserName', editable: false, editoptions: { size: dbEditWidth } },
            { key: false, name: 'UserID', key: true, hidden: true, index: 'UserID', editable: true, editoptions: { size: dbEditWidth } },
            { key: false, name: 'FirstName', index: 'FirstName', editable: true, editoptions: { size: dbEditWidth } },
            { key: false, name: 'LastName', index: 'LastName', editable: true, editoptions: { size: dbEditWidth } },
            { key: false, name: 'EmailAddress', index: 'EmailAddress', editable: true, editoptions: { size: dbEditWidth } },
            { key: false, name: 'Password', index: 'Password', editable: true, editoptions: { size: dbEditWidth } },
            {
                key: false, name: 'Active', index: 'Active', editable: true
                , formatoptions: { disabled: true }, edittype: "checkbox"
                , editoptions: { value: "Y:N" }
            }],
        pager: jQuery('#pager'),
        ondblClickRow: function (rowid) {
            jQuery(this).jqGrid('editGridRow', rowid, editOptions);
        },
        rowNum: 10,
        rowList: [2, 5, 10, 15],
        height: '100%',
        viewrecords: true,
        caption: '',
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