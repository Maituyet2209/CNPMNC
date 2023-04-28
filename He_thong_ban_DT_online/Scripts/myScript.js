
    < !--javascrip slide show-- >
    <script>
        var slideIndex = 0;
        var timer = null;
        showSlides(slideIndex);

        function plusSlides(n) {
            clearTimeout(timer);
            // if (n<0)
            // n--;
            if (n==-1)
            n=0;
          showSlides(slideIndex += n);
        }

        function currentSlide(n) {
            clearTimeout(timer);
          showSlides(slideIndex = n);
        }


        function showSlides(n) {
          var i;
          var slides = document.getElementsByClassName("myslides_sp");
          var dots = document.getElementsByClassName("dot_sp");
        //   if (n==undefined){n = ++slideIndex}
          if (n > slides.length) {slideIndex = 1}
          if (n < 1) {slideIndex = slides.length}
          for (i = 0; i < slides.length; {
            slides[i].style.display = "none";
          }
             slideIndex++;
          if (slideIndex > slides.length) {slideIndex = 1}
          for (i = 0; i < dots.length; {
            dots[i].className = dots[i].className.replace(" active", "");
            }
          slides[slideIndex-1].style.display = "block";
          dots[slideIndex-1].className += " active";
          timer = setTimeout(showSlides, 2500); // Change image every 2 seconds
        }
        </script>