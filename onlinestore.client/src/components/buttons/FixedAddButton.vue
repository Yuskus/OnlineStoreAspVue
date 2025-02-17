<template>
    <div class="fixed-add-bttn" @click="pressed()">
        <h1>+</h1>
    </div>
</template>

<script>
export default {
    methods: {
        pressed() {
            this.$emit('button-pressed');
        }
    }
}
</script>

<style scoped>
.fixed-add-bttn {
    background-color: cornflowerblue;
    width: 100px;
    height: 100px;
    border-radius: 50px;
    position: fixed;
    bottom: 10vh;
    right: 10vw;
    padding: 20px;
    box-shadow: 2px 2px 4px rgba(0, 10, 20, 0.4);
}

.fixed-add-bttn:hover {
    background-color: rgb(125, 163, 233);
    box-shadow: 4px 4px 5px rgba(0, 10, 20, 0.3);
    cursor: pointer;
}

.fixed-add-bttn:active {
    background-color: rgb(137, 163, 211);
    box-shadow: 3px 3px 5px rgba(0, 10, 20, 0.35);
    cursor: pointer;
}

.fixed-add-bttn h1 {
    color: #2d3e54;
    font-size: 48px;
    text-align: center;
}
</style>