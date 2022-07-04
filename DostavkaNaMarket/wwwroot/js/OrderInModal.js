function OrderModal() {
    var Row = document.getElementById("1");
    var Cells = Row.getElementsByTagName("td");
    var Amount = Cells[2].innerText;
    return Amount;
}