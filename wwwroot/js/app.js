window.leaveApp = {
    downloadBase64: function (filename, base64, contentType) {
        const raw = atob(base64);
        const bytes = new Uint8Array(raw.length);
        for (let i = 0; i < raw.length; i++) bytes[i] = raw.charCodeAt(i);
        const blob = new Blob([bytes], { type: contentType });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        link.remove();
        setTimeout(() => URL.revokeObjectURL(url), 1500);
    },
    printPaper: function (elementId) {
        const paper = document.getElementById(elementId);
        if (!paper) return;
        const frame = document.createElement('iframe');
        frame.style.position = 'fixed';
        frame.style.right = '0';
        frame.style.bottom = '0';
        frame.style.width = '0';
        frame.style.height = '0';
        frame.style.border = '0';
        document.body.appendChild(frame);
        const doc = frame.contentWindow.document;
        doc.open();
        doc.write('<!DOCTYPE html><html lang="vi"><head><meta charset="utf-8"><title>Đơn xin nghỉ phép</title><style>' +
            '@page { size: A4; margin: 17mm; } body { font: 13pt "Times New Roman", serif; color:#000; }' +
            '.official-paper { width: 100%; line-height:1.4; } .paper-heading { text-align:center; display:flex; flex-direction:column; }' +
            '.national-motto { text-decoration:underline; text-underline-offset:6px; } .paper-dateline { text-align:right; font-style:italic; margin:24px 0; }' +
            'h2 { text-align:center; margin:30px 0; font-size:18pt; } .paper-greeting { display:flex; gap:20px; margin:18px 0; }' +
            'p { margin:12px 0; } .paper-right { float:right; } .indented { margin-left:30px; } .paper-indent { text-indent:30px; }' +
            '.paper-underlined { border-bottom:1px dotted #000; } .signature-grid { display:grid; grid-template-columns:repeat(3,1fr); margin-top:40px; text-align:center; gap:12px; }' +
            '.signature-grid small { display:block; font-size:10pt; font-style:italic; margin-top:4px; } .signature-grid strong { display:block; margin-top:55px; }' +
            '.paper-footnote { font-size:10pt; margin-top:80px; }' +
            '</style></head><body>' + paper.outerHTML + '</body></html>');
        doc.close();
        frame.contentWindow.focus();
        setTimeout(() => {
            try { frame.contentWindow.print(); } finally { setTimeout(() => frame.remove(), 1500); }
        }, 250);
    }
};
