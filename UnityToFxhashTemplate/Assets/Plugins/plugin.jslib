var plugin = {
  GetFxhash: function () {
    var textToPass = getFxhash();
    var bufferSize = lengthBytesUTF8(textToPass) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(textToPass, buffer, bufferSize);
    return buffer;
  },
  GetFxrand: function () {
    return getFxrand();
  },
  GetIsFxpreview: function () {
    return getIsFxpreview();
  },
  TriggerFxpreview: function () {
    triggerFxpreview();
  },
  GetFxfeature: function (text) {
    var textToPass = getFxfeature(UTF8ToString(text));
    var bufferSize = lengthBytesUTF8(textToPass) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(textToPass, buffer, bufferSize);
    return buffer;
  },
  TakeScreenshot : function(array, size, fileNamePtr){
    var fileName = UTF8ToString(fileNamePtr);
 
    var bytes = new Uint8Array(size);
    for (var i = 0; i < size; i++){
       bytes[i] = HEAPU8[array + i];
    }
 
    var blob = new Blob([bytes]);
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    link.download = fileName;
 
    var event = document.createEvent("MouseEvents");
    event.initMouseEvent("click");
    link.dispatchEvent(event);
    window.URL.revokeObjectURL(link.href);
  },
};

mergeInto(LibraryManager.library, plugin);

    