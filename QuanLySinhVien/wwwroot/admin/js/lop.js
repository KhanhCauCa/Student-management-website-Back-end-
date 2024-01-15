function deleteLop(id) {
    Swal.fire({
        title: 'Bạn có chắc muốn xóa nghành này ?',
        showCancelButton: true,
        confirmButtonText: 'Có',
        denyButtonText: `Không`,
    }).then((result) => {

        if (result.isConfirmed) {
            var data = {
                id: id,

            };
            // Make an AJAX POST request to the server
            $.ajax({
                type: "POST",
                url: "/Lop/Delete", // Replace with the actual controller and action URL
                data: data,
                dataType: "json",
                success: function (result) {
                    if (result.Status == 1) {
                        MessageSucces(result.Mess);
                        location.reload();
                    } else {
                        MessageError(result.Mess);
                    }
                }
            });

        }
    })

}