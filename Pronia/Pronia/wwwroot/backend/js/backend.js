const loadMoreBtn = document.getElementById("loadMoreBtn");
const productList = document.getElementById("productList");
const productCount = document.getElementById("productCount").value;

let skip = 8;
loadMoreBtn.addEventListener("click", function () {
    console.log(skip);
    fetch(`/Shop/LoadMore?skip=${skip}`).then(response => response.text())
        .then(data => {
            productList.innerHTML += data;
        })

    skip += 8;

    if (skip >= productCount) {
        loadMoreBtn.remove();
    }
})