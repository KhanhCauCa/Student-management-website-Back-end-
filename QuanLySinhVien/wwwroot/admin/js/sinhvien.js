$(document).ready(function () {
    $('#file').change(function () {
        var input = this;

        if (input.files && input.files[0]) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#previewImage').attr('src', e.target.result);
                $('#previewImage').show();
            };
            reader.readAsDataURL(input.files[0]);
        } else {
            $('#previewImage').attr('src', '#'); // Xóa hình ảnh nếu không có tệp nào được chọn
            $('#previewImage').hide();
        }
    });
    $('.data_sinhvien').chosen();
    $('.data_monhoc').chosen();
});

function deleteSinhVien(id) {
    Swal.fire({
        title: 'Bạn có chắc muốn xóa sinh viên này không  ?',
        showCancelButton: true,
        confirmButtonText: 'Có',
        denyButtonText: `Không`,
    }).then((result) => {

        if (result.isConfirmed) {
            var data = {
                masv: id,

            };
            // Make an AJAX POST request to the server
            $.ajax({
                type: "POST",
                url: "/SinhVien/Delete", // Replace with the actual controller and action URL
                data: data,
                dataType: "json",
                success: function (result) {
                    if (result === 1) {
                      /*  MessageSucces("Xóa thành công");*/
                        location.reload();
                    } else {
                        MessageError("Xóa thất bại");
                    }
                }
            });

        }
    })
}