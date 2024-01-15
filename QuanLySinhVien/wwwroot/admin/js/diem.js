
$(document).ready(function () {
    loadData();
    $('#select_lop').chosen();

});
 function onUpdateDiem(id) {
     var data = {
         madiem: id,
     };
     
     $.ajax({
         type: "GET",
         url: "/Diem/GetDiem",
         data: data,
         success: function (result) {
             console.log(result);

             $('#madiem_update').val(result.maDiem);
             $('#sinhvien_value').val(result.tenSv);
             $('#monhoc_value').val(result.tenMonHoc);
             $('#diem_update').val(result.diem);
             
             $('#modalUpdateDiem').modal('show');
         }
     });


     



}

function UpdateDiem() {

    var diem = $("#diem_update").val();
    var madiem = $("#madiem_update").val();
    
    // Kiểm tra nếu điểm không hợp lệ
     if (diem < 1 || diem > 10) {
         $("#messUpdateDiem").text("Điểm phải nằm trong khoảng từ 1 đến 10");
    } else {
        // Xóa thông báo lỗi nếu tất cả đều hợp lệ
         $("#messUpdateDiem").text("");

        var formData = {
            madiem: madiem,
            diemthi: diem,      
        };
        $.ajax({
            type: "POST",
            url: "/Diem/UpdateDiem",
            data: formData,
            success: function (result) {
                if (result == 1) {
                    MessageSucces('Sửa điểm thành công');
                    loadData();
                    $('#modalUpdateDiem').modal('hide')
                }else {
                    MessageError('Sửa điểm thất bại')
                }
            }
        });




    }
}
function deleteDiem(id) {
    Swal.fire({
        title: 'Bạn có chắc muốn xóa điểm này ?',
        showCancelButton: true,
        confirmButtonText: 'Có',
        denyButtonText: `Không`,
    }).then((result) => {

        if (result.isConfirmed) {
            var data = {
                madiem: id,

            };
            // Make an AJAX POST request to the server
            $.ajax({
                type: "POST",
                url: "/Diem/DeleteDiem", // Replace with the actual controller and action URL
                data: data,
                dataType: "json",
                success: function (result) {
                    if (result === 1) {
                        MessageSucces("Xóa thành công");
                        loadData();
                    } else {
                        MessageError("Xóa thất bại");
                    }
                }
            });

        }
    })

}
function addDiem() {

    // Lấy giá trị của hai trường select và điểm
    var sinhVien = $(".data_sinhvien").val();
    var monHoc = $(".data_monhoc").val();
    var diem = $("#data_diem").val();


    // Kiểm tra nếu một trong hai trường select chưa được chọn
    if (sinhVien === null || monHoc === null) {
        $("#messAddDiem").text("Vui lòng chọn cả sinh viên và môn học");
    }
    // Kiểm tra nếu điểm không hợp lệ
    else if (diem < 1 || diem > 10) {
        $("#messAddDiem").text("Điểm phải nằm trong khoảng từ 1 đến 10");
    } else {
        // Xóa thông báo lỗi nếu tất cả đều hợp lệ
        $("#messAddDiem").text("");

        var formData = {
            masv: sinhVien,
            MaMonHoc: monHoc,
            DiemThi: diem
        };
        $.ajax({
            type: "POST",
            url: "/Diem/AddDiem",
            data: formData,
            success: function (result) {
                if (result == 1) {
                    MessageSucces('Nhập điểm thành công');
                    loadData();
                } else if (result == 3) {
                    let textsv = $(".data_sinhvien").text();
                    let textmh = $(".data_diem").text();
                    $("#messAddDiem").text(`Điểm của ${textsv} đã được nhập ở ${textmh}`);

                } else {
                    MessageError('Nhập điểm thất bại')
                }
            }
        });




    }



}
function loadData() {
    $('#tblDiem').dataTable().fnDestroy();
    $('#tblDiem').DataTable({
        "ajax": {
            "url": "/Diem/GetListDiem",
            "type": "GET",
            "dataType": "json",
            "dataSrc": ""
        },
        "columns": [
            { "data": "MaSv" },
            { "data": "TenSv" },
            { "data": "TenMonHoc" },
            { "data": "Diem" },
            {
                "data": null, "render": function (data, type, row) {
                    return `<button type="button" class="btn btn-warning" onclick="return onUpdateDiem('${row.MaDiem}')" ><i class="fas fa-edit"></i></i></button>
                        <button type="button" class="btn btn-danger" onclick="return deleteDiem('${row.MaDiem}')" ><i class="fas fa-trash"></i></button>`;
                }
            },

        ]
    });
}

