// Function to handle form submissions for offers
function submitFormOffer(page) {
    // Retrieve selected filter options
    var selectCountry = document.getElementById("selectCountry");
    var country = selectCountry.options[selectCountry.selectedIndex].value;

    var selectPrice = document.getElementById("priceOrder");
    var order = selectPrice.options[selectPrice.selectedIndex].value;

    var selectCityFrom = document.getElementById("selectFrom");
    var cityFrom = selectCityFrom.options[selectCityFrom.selectedIndex].value;

    var selectCityTo = document.getElementById("selectTo");
    var cityTo = selectCityTo.options[selectCityTo.selectedIndex].value;

    // Construct URL query string based on selected options and page number
    var url = "?"
    if (country != "All") { url += 'countrySearch=' + country + '&' };
    if (order != "") { url += 'sortOrder=' + order + '&' };
    if (cityFrom != "All") { url += 'cityFrom=' + cityFrom + '&' };
    if (cityTo != "All") { url += 'cityTo=' + cityTo + '&' };

    // Append page number to the URL
    var queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    var actual_page = parseInt(urlParams.get('page'));

    if (page == 0 || isNaN(actual_page)) { url += 'page=1'; actual_page = 1}
    else if (page == 1) { url += 'page=' + (actual_page + 1) }
    else if (page == -1) { url += 'page=' + (actual_page - 1) }

    // Redirect user to the new URL
    window.location.replace(url);
};

// Function to handle form submissions for hotels
function submitFormHotel(page) {
    // Retrieve selected filter options
    var selectRate = document.getElementById("rateOrder");
    var order = selectRate.options[selectRate.selectedIndex].value;

    var selectCity = document.getElementById("selectCity");
    var city = selectCity.options[selectCity.selectedIndex].value;

    // Construct URL query string based on selected options and page number
    var url = "?"
    if (order != "") { url += 'sortOrder=' + order + '&' };
    if (city != "All") { url += 'citySearch=' + city + '&' };

    var actual_page = parseInt(window.location.href.slice(-1));
    if (page == 0 || isNaN(actual_page) || actual_page == 'N') { url += 'page=1' }
    else if (page == 1) { url += 'page=' + (actual_page + 1) }
    else if (page == -1) { url += 'page=' + (actual_page - 1) }

    // Redirect user to the new URL
    window.location.replace(url);
};

// Function to handle form submissions for cities
function submitFormCity(page) {
    // Retrieve selected filter options
    var selectRate = document.getElementById("sortOrder");
    var order = selectRate.options[selectRate.selectedIndex].value;

    var selectCity = document.getElementById("selectCountry");
    var city = selectCity.options[selectCity.selectedIndex].value;

    // Construct URL query string based on selected options and page number
    var url = "?"
    if (city != "All") { url += 'countrySearch=' + city + '&' };
    if (order != "") { url += 'sortOrder=' + order + '&' };

    var actual_page = parseInt(window.location.href.slice(-1));
    if (page == 0 || isNaN(actual_page) || actual_page == 'N') { url += 'page=1' }
    else if (page == 1) { url += 'page=' + (actual_page + 1) }
    else if (page == -1) { url += 'page=' + (actual_page - 1) }

    // Redirect user to the new URL
    window.location.replace(url);
};

// Function to handle form submissions for countries
function submitFormCountry(page) {
    // Retrieve selected filter options
    var selectRate = document.getElementById("sortOrder");
    var order = selectRate.options[selectRate.selectedIndex].value;

    // Construct URL query string based on selected options and page number
    var url = "?"
    if (order != "") { url += 'sortOrder=' + order + '&' };

    var actual_page = parseInt(window.location.href.slice(-1));

    if (page == 0 || isNaN(actual_page) || actual_page == 'N') { url += 'page=1' }
    else if (page == 1) { url += 'page=' + (actual_page + 1) }
    else if (page == -1) { url += 'page=' + (actual_page - 1) }

    // Redirect user to the new URL
    window.location.replace(url);
};