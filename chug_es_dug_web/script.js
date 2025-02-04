let four=document.getElementById('four');
let three=document.getElementById('three');
let two=document.getElementById('two');
let text=document.getElementById('text');
let btn=document.getElementById('btn');
let one=document.getElementById('one');
let nullfive=document.getElementById('nullfive');
let nulll=document.getElementById('nulll');
let sec=document.getElementById('sec');


window.addEventListener('scroll',function(){
	let value=window.scrollY;
	four.style.top=0.18*value+'px';
	three.style.top=0.19*value+'px';
	two.style.top=0.2*value+'px';
	one.style.top=0.21*value+'px';
	nullfive.style.top=-0.22*value+'px';
	nulll.style.top=-0.24*value+'px';   

	function responsive(x) {
				if (x.matches) { // If media query matches
					sec.style.top=0.1*value+90+'px';
				} else {
					sec.style.top=0.001*value+140+'px';
				}
		}

	var x = window.matchMedia("(max-width: 750px)")
	responsive(x) // Call listener function at run time
	x.addListener(responsive) // Attach listener function on state changes

})
