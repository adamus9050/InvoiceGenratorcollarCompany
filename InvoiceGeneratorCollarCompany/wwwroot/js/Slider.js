
var numer = Math.floor(Math.random() * 2) + 1;
function zmienslajd()
{
    numer++;
    if (numer > 2) numer = 1;

    var plik = "<img src=\"InvoiceGeneratorCollarCompany\wwwroot\Image\" + numer + ".jpg\" />"

    document.getElementById("slider").innerHTML = plik;

    setTimeout("zmienslajd()",5000)
}