const atCountStyle = {
    Zero: 'btn btn-sm btn-light text-muted disabled',
    Total: 'btn btn-sm btn-outline-primary',
    Present: 'btn btn-sm btn-outline-success',
    Absent: 'btn btn-sm btn-outline-danger',
    Malpractice: 'btn btn-sm btn-outline-warning',
    Buffer: 'btn btn-sm btn-outline-info',
    Pending: 'btn btn-sm btn-outline-secondary',
    IOPending: 'btn btn-sm btn-warning',
    ScanPending: 'btn btn-sm btn-danger'
}

$(document).ready(function () {
    $('.at-row').each(function () {
        var tr = $(this);

        let at_total = tr.find('a.at-total')
        at_total.addClass(at_total.text() == 0 ? atCountStyle.Zero : atCountStyle.Total);

        let at_present = tr.find('a.at-present')
        at_present.addClass(at_present.text() == 0 ? atCountStyle.Zero : atCountStyle.Present);

        let at_absent = tr.find('a.at-absent')
        at_absent.addClass(at_absent.text() == 0 ? atCountStyle.Zero : atCountStyle.Absent);

        let at_malpractice = tr.find('a.at-malpractice')
        at_malpractice.addClass(at_malpractice.text() == 0 ? atCountStyle.Zero : atCountStyle.Malpractice);

        let at_buffer = tr.find('a.at-buffer')
        at_buffer.addClass(at_buffer.text() == 0 ? atCountStyle.Zero : atCountStyle.Buffer);

        let at_pending = tr.find('a.at-pending')
        at_pending.addClass(at_pending.text() == 0 ? atCountStyle.Zero : atCountStyle.Pending);

        let at_io_pending = tr.find('a.at-io-pending')
        at_io_pending.addClass(at_io_pending.text() == 0 ? atCountStyle.Zero : atCountStyle.IOPending);

        let at_scan_pending = tr.find('a.at-scan-pending')
        at_scan_pending.addClass(at_scan_pending.text() == 0 ? atCountStyle.Zero : atCountStyle.ScanPending);
    });
});