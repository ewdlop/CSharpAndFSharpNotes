window.clientEventListener = {
    browserResize: (dotNetObjectReference) => {
        window.onresize = () => {
            let browserWidth = window.innerWidth;
            let browserHeight = window.innerHeight;
            let dto = {
                clientX: browserWidth,
                clientY: browserHeight
            }
            dotNetObjectReference.invokeMethodAsync('OnBrowserResize', dto);
        };
    },

    getDimensionsById: (Id) => {
        let divElement = document.getElementById(Id);
        if (divElement) {
            let divHeight = divElement.clientHeight;
            let divWidth = divElement.clientWidth;
            let dto = {
                clientX: divWidth,
                clientY: divHeight
            }
            return JSON.stringify(dto);
        }
        return JSON.stringify({
            clientX: -1,
            clientY: -1
        });
    },

    divIdResize: (dotNetObjectReference, divId) => {
        window.onresize = () => {
            let divElement = document.getElementById(divId);
            if (divElement) {
                let divHeight = divElement.clientHeight;
                let divWidth = divElement.clientWidth;
                let dto = {
                    clientX: divWidth,
                    clientY: divHeight
                }
                dotNetObjectReference.invokeMethodAsync('OnDivResize', dto);
            }
        };
    }
}