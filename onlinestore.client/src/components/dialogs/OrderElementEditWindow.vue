<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать единицу заказа</h2>

            <DialogLineForm v-model="localItem.itemsCount" :labelName="'Количество'" :inputType="'number'" :inputText="localItem.itemsCount" />
    
            <div class="buttons">
                <button @click="applyChanges()">Изменить</button>
                <button @click="deleteItem()">Удалить</button>
                <button @click="cancelDialog()">Отмена</button>
            </div>
        </div>
    </div>
</template>

<script>
import { updateOrderElement, deleteOrderElement } from '../../api/orderElementsApi';

import DialogLineForm from '../forms/DialogLineForm.vue';

export default {
    components: { DialogLineForm },
    props: {
        item: {
            type: Object
        }
    },
    data() {
        return {
            localItem: { ...this.item }
        }
    },
    methods: {
        makeOrderElementRequestBody() {
            return {
                orderId: this.localItem.orderId,
                itemId: this.localItem.itemId,
                itemsCount: this.localItem.itemsCount,
                itemPrice: this.localItem.itemPrice
            };
        },
        async applyChanges() {
            try {
                let newOrderElement = this.makeOrderElementRequestBody();
                const response = await updateOrderElement(this.localItem.id, newOrderElement);
                if (!response) {
                    alert('Ошибка при изменении элемента заказа (OrderElement).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при изменении данных (OrderElementsEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        async deleteItem() {
            try {
                const response = await deleteOrderElement(this.localItem.id);
                if (!response) {
                    alert('Ошибка при удалении элемента заказа (OrderElement).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при удалении данных (OrderElementsEdit): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        warnInfo(message, error) {
            console.error(message, error);
            alert('Проблемы с сервером!');
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

.form label {
    width: 50%;
    margin-right: 10px;
    padding: 10px;
}

.form input {
    width: 50%;
    padding: 10px;
    border-radius: 10px;
    border: 1px solid rgba(0,0,80,0.5);
}

h2 {
    color: #212933;
}

.buttons {
    width: fit-content;
    margin-top: 20px;
    right: 0px;
    float: right;
}

.buttons button {
    margin: 0 10px;
    padding: 10px;
    border-radius: 10px;
    border: none;
    background-color: rgba(0,0,30,0.1);
}

.buttons button:hover {
    background-color: rgba(0,0,30,0.25);
}

.buttons button:focus {
    background-color: rgba(0,0,30,0.4);
}

.form {
    display: flex;
    margin-bottom: 15px;
}

hr {
    border: 1px dashed rgb(141, 169, 221, 0.5);
    margin: 15px 0px;
}
</style>