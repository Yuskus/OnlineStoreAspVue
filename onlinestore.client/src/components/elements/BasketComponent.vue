<template>
    <div>
        <div class="table">
            <div class="row colored">
                <div class="cell">Миниатюра</div>
                <div class="cell">Название</div>
                <div class="cell">Категория</div>
                <div class="cell">Количество</div>
                <div class="cell">Цена за шт.</div>
                <div class="cell">Опции</div>
            </div>
            <div class="row" v-for="(item, index) in basket" :key="index" >
                <div class="cell"><img src="../../assets/item.jpg" width="80" height="80" /></div>
                <div class="cell">{{ item.itemResponse.name }}</div>
                <div class="cell">{{ item.itemResponse.category }}</div>
                <div class="cell">{{ item.itemsCount }}</div>
                <div class="cell">{{ item.itemResponse.price }}</div>
                <div class="cell"><a class="accent" @click="clickOnItem(index)">Редактировать</a></div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        basket: {
            type: Array,
            required: true
        },
        isOpen: {
            type: Boolean,
            required: true
        }
    },
    data() {
        return {
            selectedItem: null
        }
    },
    methods: {
        async clickOnItem(index) {
            this.selectedItem = this.basket[index];
            this.$emit('update-values', { item: this.selectedItem, isOpen: !this.isOpen });
        }
    }
}
</script>

<style scoped>
  * {
    font-weight: 500;
    color: #1c2633;
  }

  .table {
    display: table;
    width: 100%;
    border-collapse: collapse;
  }

  .row {
    display: table-row;
  }

  .cell {
    display: table-cell;
    border-top: 1px solid #6678b1;
    border-bottom: 1px solid #6678b1;
    padding: 10px;
    text-align: left;
    font-size: 18px;
    background-color: white;
    vertical-align: middle;
  }

  .colored {
    font-weight: bold;
    background-color: rgba(163, 194, 232, 1);
  }

  .accent {
    text-decoration: underline;
  }

  .accent:hover {
    color: #34547a;
  }
</style>