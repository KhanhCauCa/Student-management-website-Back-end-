//Hiển thị sweet alert lỗi
function MessageError(text) {
     Swal.fire({
        title: "Thông báo",
        icon: 'error',
        text: text,
        button: false,
        //timer: 2000
    });
}

//Hiển thị sweet alert thành công
function MessageSucces(text) {
     Swal.fire({
        title: 'Thông báo',
        text: text,
        button: false,
        icon: 'success',
        timer: 1000
    });
}

//Hiển thị sweet alert cảnh báo
function MessageWarning(text, timer) {
     Swal.fire({
        title: 'Thông báo',
        text: text,
        button: false,
        icon: 'warning',
        timer: timer
    });
}