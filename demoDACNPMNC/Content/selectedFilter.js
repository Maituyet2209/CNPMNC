
//filter price
var subfilters = document.querySelectorAll(".filter_price li a");
if (subfilters.length == 0) {
    console.log("Không có thẻ a nào được tìm thấy");
} else {
    console.log("Số thẻ a tìm thấy: " + subfilters.length);
    for (var temp = 0; temp < subfilters.length; temp++) {
        subfilters[temp].addEventListener("click", selectedFilter);
    }
}

function selectedFilter(event) {
    var getText = event.target;
    localStorage.setItem("selectedFilter", getText.textContent.trim());
    document.cookie = "selectedFilter=" + localStorage.getItem("selectedFilter") + "; path=/"; // lưu vào Cookie
}

// filter ram filter_ram
var filter_ram = document.querySelectorAll(".filter_ram li a");
for (var temp = 0; temp < filter_ram.length; temp++) {
    filter_ram[temp].addEventListener("click", selectedFilter_ram);
}

function selectedFilter_ram(event) {
    var getText = event.target;
    localStorage.setItem("filter_ram", getText.textContent.trim());
    document.cookie = "filter_ram=" + localStorage.getItem("filter_ram") + "; path=/"; // lưu vào Cookie
}

// filter ram filter_rom
var filter_rom = document.querySelectorAll(".filter_rom li a");
for (var temp = 0; temp < filter_rom.length; temp++) {
    filter_rom[temp].addEventListener("click", selectedFilter_rom);
}

function selectedFilter_rom(event) {
    var getText = event.target;
    localStorage.setItem("filter_rom", getText.textContent.trim());
    document.cookie = "filter_rom=" + localStorage.getItem("filter_rom") + "; path=/"; // lưu vào Cookie
}



//$(document).ready(function () {
//    $(".sub_filter li a").click(function () {
//        // Lấy nội dung của thẻ li
//        var filterValue = $(this).text().trim();

//        // Lưu vào localStorage với key là "selectedFilter" và giá trị là filterValue
//        localStorage.setItem("selectedFilter", filterValue);
//    });
//});
