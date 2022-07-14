function BlazorDownloadFile(fileName, byteBase64) {
    var link = document.createElement('a');
    
    link.href = "data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64," + byteBase64;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
