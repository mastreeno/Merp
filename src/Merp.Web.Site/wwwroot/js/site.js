// Write your Javascript code.
function TransformDate(date)
{
    var parts = date.split('-');
    var mydate = new Date(Date.UTC(parts[2], parts[1] - 1, parts[0]));

    return mydate;
}