<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать список товаров</h2>

            <BasketComponent :basket="basket" :isOpen="isOpenDialog" @update-values="updateFromBasket" />
    
            <div class="buttons">
                <button @click="applyChanges()">Изменить</button>
                <button @click="deleteItem()">Удалить</button>
                <button @click="cancelDialog()">Отмена</button>
            </div>
        </div>
    </div>
</template>

<script>
import BasketComponent from '../elements/BasketComponent.vue';

export default {
    components: { BasketComponent },
    props: {
        basket: {
            type: Array,
            required: true
        }
    },
    data() {
        return {
            isOpenDialog: false
        }
    },
    methods: {
        applyChanges() {

        },
        deleteItem() {

        },
        cancelDialog() {
            this.$emit('close-dialog', false);
        }
    }
}
</script>

<style scoped>
.dialog-over {
    position: fixed;
    display: flex;
    justify-content: center;
    align-items: center;
    top: 0px;
    bottom: 0px;
    left: 0px;
    right: 0px;
    transform: scale(1.25);
    background-color: rgba(0,0,0,0.4);
    z-index: 1000;
}

.dialog {
    background-color: white;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.3);
    min-width: 400px;
    width: 50vw;
    min-height: fit-content;
}
</style>