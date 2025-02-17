<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <ItemEditWindow v-if="role === '1' && isOpenDialog === true" @close-dialog="clickWindowRedactor" :item="selectedItem" />

  <div class="filters">
      <div class="button" @click="getItems()">Все категории</div>
      <div class="button" v-for="(category, index) in categories" :key="index" @click="getByCategory(category)">{{ category }}</div>
    </div>

  <div class="container">

    <div v-if="items.length > 0" class="catalog">
      <ItemComponent v-for="(item, index) in items" 
                    :key="index" 
                    :item="item" 
                    :isBounce="isBouncing[index]" 
                    @item-click="clickOnItem(index)" />
    </div>
    <div v-else>
      <h2>Товаров нет.</h2>
    </div>

    <Pagination @page-changed="getItems" :current="currentPage" :totalPages="totalPages" />

  </div>
</template>

<script>
  import { getBasket } from '../api/ordersApi';
  import { addOrderElement } from '../api/orderElementsApi';
  import { getCategories, getPageOfItemsByCategory, getPageOfItems } from '../api/itemsApi';

  import Pagination from '../components/pagination/PaginationComponent.vue';
  import ItemComponent from '../components/elements/ItemComponent.vue';
  import ItemEditWindow from '../components/dialogs/ItemEditWindow.vue';

  export default {
    name: 'Catalog',
    components: { Pagination, ItemComponent, ItemEditWindow },
    data() {
      return {
        myId: null,
        role: '',
        items: [],
        categories: [],
        basketOrder: null,
        currentPage: 1,
        totalPages: 1,
        totalCount: 0,
        pageSize: 12,
        selectedItem: null,
        isOpenDialog: false,
        isBouncing: []
      }
    },
    methods: {
      getMyData() {
        this.myId = localStorage.getItem('guid');
        this.role = localStorage.getItem('role');
      },
      makeOrderElementRequest(item) {
        return {
          orderId: this.basketOrder.id,
          itemId: item.id,
          itemsCount: 1,
          itemPrice: item.price
        };
      },
      async getCategories() {
        try {
          const response = await getCategories();
          if (response) {
            this.categories = response;
          } else {
            alert('Ошибка во время получения категорий.');
          }
        } catch (error) {
          this.warnInfo('Ошибка во время получения данных (CatalogView).', error);
        }
      },
      async getByCategory(category, number = 1) {
        this.currentPage = number;
        try {
          const response = await getPageOfItemsByCategory(category, this.currentPage, this.pageSize);
          if (response) {
            this.items = response.responses;
            this.totalCount = response.totalCount;
            this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          } else {
            alert('Ошибка во время получения страницы товаров по выбранной категории.');
          }
        } catch (error) {
          this.warnInfo('Ошибка во время получения данных (CatalogView).', error);
        }
      },
      async getItems(number = 1) {
        this.currentPage = number;
        try {
          const response = await getPageOfItems(this.currentPage, this.pageSize);
          if (response) {
            this.items = response.responses;
            this.totalCount = response.totalCount;
            this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          } else {
            alert('Ошибка во время получения товаров.');
          }
        } catch (error) {
          this.warnInfo('Ошибка во время получения данных (CatalogView).', error);
        }
      },
      async getBasketNumber() {
        if (this.role === '1') return;
        try {
          const response = await getBasket(this.myId);
          if (response) {
            this.basketOrder = response;
          } else {
            alert('Ошибка во время получения информации о корзине.');
          }
        } catch (error) {
          this.warnInfo('Ошибка во время получения данных (CatalogView).', error);
        }
      },
      async addInBasket(item) {
        try {
          let newOrderElement = this.makeOrderElementRequest(item);
          const response = await addOrderElement(newOrderElement);
          if (!response) {
            alert('Ошибка во время добавления товара в корзину.');
          }
        } catch (error) {
          this.warnInfo('Ошибка во время добавления данных (CatalogView).', error);
        }
      },
      async clickOnItem(index) {
        if (this.role === '1') {
          this.selectedItem = this.items[index];
          await this.clickWindowRedactor(true);
        } else {
          this.isBouncing[index] = true;
          setTimeout(() => {
            this.isBouncing[index] = false;
          }, 1000);
          await this.addInBasket(this.items[index]);
        }
      },
      async clickWindowRedactor(state) {
        this.isOpenDialog = state;
        if (!state) {
          await this.refreshDataOnPage();
        }
      },
      async refreshDataOnPage() {
        await this.getCategories();
        await this.getItems();
        await this.getBasketNumber();
      },
      warnInfo(message, error) {
        console.error(message, error);
        alert(error.message);
      }
    },
    mounted() {
      this.getMyData();
      this.refreshDataOnPage();
    }
  }
</script>

<style scoped>
  * {
      font-weight: 500;
      color: #1c2633;
  }

  .container {
    max-width: 70vw;
    width: fit-content;
    margin: 0px auto;
    padding: 20px 0px 40px 0px;
  }

  .filters {
    display: block;
    max-width: 70vw;
    margin: 0 auto;
    padding: 10px;
    align-items: center;
  }

  .catalog {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    padding: 20px 0px 30px 0px;
  }

  .button {
    display: inline-block;
    justify-content: space-around;
    text-decoration: none;
    margin: 5px 10px;
    padding: 10px;
    background-color: #f0f5fa;
    border-radius: 15px;
  }

  .button:hover {
    background-color: #dce1f0;
  }
</style>

