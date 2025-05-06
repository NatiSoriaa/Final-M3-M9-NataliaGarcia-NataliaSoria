document.addEventListener("DOMContentLoaded", function () {
  document.querySelectorAll(".rating").forEach((rating) => {
    const stars = rating.querySelectorAll(".star");
    const bookId = rating.dataset.bookId;

    let currentRating = parseInt(localStorage.getItem(`rating-${bookId}`)) || 0;
    highlightStars(stars, currentRating);

    stars.forEach((star) => {
      star.addEventListener("click", () => {
        const value = parseInt(star.dataset.value);

        if (currentRating === value) {
          
          localStorage.removeItem(`rating-${bookId}`);
          currentRating = 0;
        } else {
          
          localStorage.setItem(`rating-${bookId}`, value);
          currentRating = value;
        }

        highlightStars(stars, currentRating);
      });
    });
  });

  function highlightStars(stars, value) {
    stars.forEach((star, index) => {
      star.textContent = index < value ? "★" : "☆";
    });
  }
});
