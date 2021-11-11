
window.onload = () => {
    sidebarEvent()
    expandEvent()
    popupEvent()
}

// ẩn hiện thanh sidebar
function sidebarEvent() {
    const toggle = document.querySelector('.toggle-bar')
    const sidebar = document.querySelector('.sidebar')

    toggle.onclick = e => {
        sidebar.classList.toggle('hidden');
    }
}

//phóng to full màn hình
function expandEvent() {
    const expand = document.querySelector('.expand-btn')
    const elem = document.documentElement;
    let index = 0
    expand.onclick = e => {
        index++;
        if (index % 2 != 0) {
            if (elem.requestFullscreen) {
                elem.requestFullscreen();
            } else if (elem.webkitRequestFullscreen) { /* Safari */
                elem.webkitRequestFullscreen();
            } else if (elem.msRequestFullscreen) { /* IE11 */
                elem.msRequestFullscreen();
            }
        } else {
            if (document.exitFullscreen) {
                document.exitFullscreen();
            } else if (document.webkitExitFullscreen) { /* Safari */
                document.webkitExitFullscreen();
            } else if (document.msExitFullscreen) { /* IE11 */
                document.msExitFullscreen();
            }
        }

    }
}

function popupEvent() {
    const btnVoice = document.querySelector('.btn-voice')
    btnVoice.onclick = () => {
        var msg = new SpeechSynthesisUtterance();
        msg.lang = "vi-VN";
        msg.text = " Nguyễn Hoàng Long";
        speechSynthesis.speak(msg);
    }

    const popup = document.querySelector('.popup');
    const popupMain = document.querySelector('.popup .main');
    const popupContent = document.querySelectorAll('.content_holder .content_main')

    popupContent.forEach(item => {
        item.onclick = (e) => {
            popupMain.innerHTML = `
        <div class="my_row">
                <p>Tên môn</p>
                <b>${item.querySelector('.hold .left .hiden_info .TenMon').innerText}</b>
            </div>
            <div class="my_row">
                <p>Mã môn học:</p>
                <b>${item.querySelector('.hold .left .hiden_info .MaMon').innerText}</b>
            </div>
            <div class="my_row">
                <p>Tiết bắt đầu:</p>
                <b>${item.querySelector('.hold .left .hiden_info .TietBatDau').innerText}</b>
            </div>
            <div class="my_row">
                <p>Tiết:</p>
                <b>${item.querySelector('.hold .left .hiden_info .Tiet').innerText}</b>
            </div>
            <div class="my_row">
                <p>Thứ:</p>
                <b>${item.querySelector('.hold .left .hiden_info .TietBatDau').innerText}</b>
            </div>
            <div class="my_row">
                <p>Mã phòng:</p>
                <b>${item.querySelector('.hold .left .hiden_info .MaPhong').innerText}</b>
            </div>
            <div class="my_row">
                <p>Tên phòng:</p>
                <b>${item.querySelector('.hold .left .hiden_info .TenPhong').innerText}</b>
            </div>
            <div class="my_row">
                <p>Ngày học:</p>
                <b>${item.querySelector('.hold .left .hiden_info .NgayHoc').innerText}</b>
            </div>
            <div class="my_row">
                <p>Lớp:</p>
                <b>${item.querySelector('.hold .left .hiden_info .LopMon').innerText}</b>
            </div>
            <div class="my_row">
                <p>Năm học:</p>
                <b>${new Date().getFullYear()}</b>
            </div>
 
            <div class="my_row">
                <p>Số tín chỉ:</p>
                <b>${item.querySelector('.hold .left .hiden_info .SoTinChi').innerText}</b>
            </div>
            <div class="my_row">
                <p>Số tín chỉ học phần:</p>
                <b>${item.querySelector('.hold .left .hiden_info .SoTinChi').innerText}</b>
            </div>
            <div class="my_row">
                <p>Mã giảng viên:</p>
                <b>${item.querySelector('.hold .left .hiden_info .MaGiaoVien').innerText}</b>
            </div>
            <div class="my_row">
                <p>Tên giảng viên:</p>
                <b>${item.querySelector('.hold .left .hiden_info .TenGiaoVien').innerText}</b>
            </div>
            <div class="my_row">
                <p>Nhóm học:</p>
                <b>${item.querySelector('.hold .left .hiden_info .Nhom').innerText}</b>
            </div>
        `;
            popup.classList.toggle('hidden');
        }
    });

    // ấn vào nền để tắt popup
    popup.onclick = () => {
        popup.classList.toggle('hidden');
    }

    // ngăn nổi bọt, nhấp zo main ko bị tắt popup
    popupMain.onclick = (e) => {
        e.stopPropagation();
    }
}