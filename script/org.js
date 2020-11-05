var organization = {} || employee;




organization.openAddOrg = function () {
    organization.reset();
    $('#addEditOrg').modal('show');
};

organization.reset = function () {
    $('#Name').val("");
    $('#Id').val(0);
    $('#Code').val("");
    $('#ImagePath').val("");
}

organization.uploadImage = function (input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#ImagePath').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}


//$("#uploadEditorImage").change(function () {
//    var data = new FormData();
//    var files = $("#uploadEditorImage").get(0).files;
//    if (files.length > 0) {
//        data.append("HelpSectionImages", files[0]);
//    }
//    $.ajax({
//        url: resolveUrl("~/Admin/HelpSection/AddTextEditorImage/"),
//        type: "POST",
//        processData: false,
//        contentType: false,
//        data: data,
//        success: function (response) {
//            //code after success

//        },
//        error: function (er) {
//            alert(er);
//        }

//    });
//});


organization.save = function () {
    var rvtoken = $("input[name='__RequestVerificationToken']").val();
    var siteRoot = dnn.getVar("sf_siteRoot", "/");
    var org = {};
    org.Name = $('#Name').val();
    org.Id = parseInt($('#Id').val());
    org.Code = $('#Code').val();
    //org.ImagePath = $('#ImagePath').attr('src');
    console.log(org.Id);
    console.log(org.Name);
    console.log(org.Code);
    console.log(org.ImagePath);
    var datas = new FormData();
    var files = $("#ImagePath").get(0).files;
    if (files.length > 0) {
        datas.append("HelpSectionImages", files[0]);
    }
    console.log(datas);
    $.ajax({
        url: siteRoot + 'DesktopModules/MVC/Organization/Item/SaveOrganization',
        method: "POST",
        dataType: "json",
        contentType: "application/json",
        data: { objOrg: JSON.stringify(org) },
        //data: { OrganizationId: org.Id, Name: org.Name, Code: org.Code, ImagePath: org.ImagePath },
        headers: {
            "ModuleId": ModuleId,
            "TabId": TabId,
            "RequestVerificationToken": rvtoken
        },
        success: function (data) {
            $('#addEditOrg').modal('hide');
            //bootbox.alert(data.data.message);
        }
    });
}





organization.get = function (id) {
    var rvtoken = $("input[name='__RequestVerificationToken']").val();
    var ModuleId = $('#ModuleId').val();
    var TabId = $('#TabId').val();
    var siteRoot = dnn.getVar("sf_siteRoot", "/");
    $.ajax({
        url: siteRoot + 'DesktopModules/MVC/Organization/Item/GetOrganization',
        method: "GET",
        dataType: "json",
        data: { organizationId: id },
        headers: {
            "ModuleId": ModuleId,
            "TabId": TabId,
            "RequestVerificationToken": rvtoken
        },
        success: function (data) {
            console.log(data.data)
            var obj = JSON.parse(data.data);
            $('#desOrg #Name').val(obj.Name);
            $('#desOrg').find('#Id').text(id);
            $('#desOrg #Code').val(obj.Code);
            $('#desOrg #ImagePath').attr("src", obj.ImagePath);
            $('#addEditOrg').modal('show');
        }
    });
}




organization.delete = function (id) {
    //alert(id);
    var rvtoken = $("input[name='__RequestVerificationToken']").val();
    var ModuleId = $('#ModuleId').val();
    var TabId = $('#TabId').val();
    var siteRoot = dnn.getVar("sf_siteRoot", "/");
    bootbox.confirm({
        title: "Delete organization?",
        message: "Do you want to delete this organization.",
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> No'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Yes'
            }
        },
        callback: function (data) {
            if (data) {
                $.ajax({
                    url: siteRoot + 'DesktopModules/MVC/Organization/Item/DeleteOrganization',
                    method: "GET",
                    dataType: "json",
                    data: { organizationId: id },
                    headers: {
                        "ModuleId": ModuleId,
                        "TabId": TabId,
                        "RequestVerificationToken": rvtoken
                    },
                    success: function (data) {
                        //var obj = JSON.parse(data.data);
                         alert(data.data);
                        //alert(obj.message);
                        bootbox.alert(data.data.message);
                    }
                });
            }
        }
    });
}


