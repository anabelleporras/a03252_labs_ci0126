<template>
  <div class="coffee-page">
    <h1 class="title">La vida es corta, bebe un buen café.</h1>

    <div v-if="outOfOrderMessage" class="oor-message">
      {{ outOfOrderMessage }}
    </div>

    <div class="subtitle">Ingrese la cantidad de cafes deseados:</div>

    <div class="coffee-list">
      <div
        v-for="coffee in coffees"
        :key="coffee.name"
        class="coffee-row"
      >
        <button class="coffee-button">
          <span>{{ coffee.name }}</span>
          <span>₡ {{ coffee.price }}</span>
          <span>Disponible: {{ coffee.available }}</span>
        </button>

        <input
          type="number"
          :disabled="coffee.available <= 0"
          min="0"
          :max="coffee.available"
          class="qty-input"
          v-model.number="coffee.qty"
        />
      </div>
    </div>

    <div class="order-box">
      <h2 class="order-title">Su Orden</h2>
      <div class="order-inner">
        <div class="order-section">
          <strong>Orden:</strong>
          <ul v-if="orderLines.length">
            <li v-for="item in orderLines" :key="item.name">
              {{ item.qty }} × {{ item.name }}
            </li>
          </ul>
          <p v-else class="empty-text">Sin productos seleccionados</p>
        </div>

        <div class="order-section total">
          <strong>Total a Pagar:</strong>
          <span>₡ {{ totalToPay }}</span>
        </div>
      </div>
    </div>

    <div class="subtitle mt">Seleccione las denominaciones de pago:</div>

    <div class="payment-row">
      <div class="payment-buttons">
        <button
          v-for="value in paymentOptions"
          :key="value"
          class="payment-button"
          @click="addPayment(value)"
        >
          {{ value }}
        </button>
      </div>

      <div class="payment-display">
        <span v-if="paymentTotal">Pago: ₡ {{ paymentTotal }}</span>
      </div>
    </div>

    <div class="confirm-row">
      <button class="confirm-button" @click="buyCoffee">
        Confirmar orden
      </button>
    </div>

    <div v-if="responseMessage" class="response-message">
      {{ responseMessage }}
    </div>
  </div>
</template>

<script>
import urlBaseAPI from '@/axios'
export default {
  name: 'CoffeeMachine',
  data() {
    return {
      coffees: [],
      paymentOptions: [1000, 500, 100, 50, 25],
      paymentTotal: 0,
      paymentDenominations: [],
      responseMessage: '',
      errorMessage: '',
      outOfOrderMessage: ''
    };
  },

  async mounted() {
    await this.loadCoins();
    await this.loadCoffees();
    
  },

  computed: {
    totalToPay() {
      return this.coffees.reduce(
        (sum, c) => sum + c.price * (c.qty || 0),
        0
      );
    },
    orderLines() {
      return this.coffees.filter(c => c.qty > 0);
    },
  },
  methods: {
    async loadCoffees() {
      this.errorMessage = "";
      try {
        const response = await urlBaseAPI.get("/api/CoffeeMachine");
        const data = response.data.result;

        this.coffees = Object.keys(data).map(key => ({
          name: key,
          price: data[key].price,
          available: data[key].quantity,
          qty: 0
        }));
      } catch (err) {
        this.errorMessage = "Error cargando cafés: " + err;
      }
    },

    addPayment(amount) {
      this.paymentTotal += amount;
      this.paymentDenominations.push(amount);
    },

    async loadCoins() {
      this.outOfOrderMessage = "";
      const response = await urlBaseAPI.get("/api/CoffeeMachine/Coin");
      const data = response.data.result;
      for (const [coin, qty] of Object.entries(data)) {
        if(qty === 0) {
          this.outOfOrderMessage = "Fuera de Servicio: no hay sufientes monedas de " + coin + " colones";
          break;
        }
      }
    },

    async buyCoffee() {
      this.responseMessage = "";

      if (!this.orderLines.length) {
        this.responseMessage = 'No ha seleccionado ningún café.';
        return;
      }
      if (this.paymentTotal < this.totalToPay) {
        this.responseMessage = "El pago es insuficiente.";
        return;
      }
      
      const order = {};
      this.orderLines.forEach(c => {
        order[c.name] = c.qty;
      });

      const billsCount = this.paymentDenominations.filter(v => v === 1000).length;
      const bills = billsCount ? [billsCount] : [];

      const coins = {};
      this.paymentDenominations
        .filter(v => v !== 1000)
        .forEach(v => {
          const key = String(v);
          coins[key] = (coins[key] || 0) + 1;
        });

      const payload = {
        order,
        payment: {
          totalAmount: this.paymentTotal,
          bills,
          coins
        }
      };

      try {
        const response = await urlBaseAPI.post("/api/CoffeeMachine", payload);

        this.responseMessage = response.data?.result;

        this.reset();
      } catch {
        this.responseMessage = "Ocurrió un error inesperado.";
      }
    },

    reset() {
      this.coffees.forEach(c => (c.qty = 0));
      this.paymentTotal = 0;
      this.paymentDenominations = [];
      this.loadCoins();
      this.loadCoffees();
    }
  }
};
</script>

<style scoped>
.coffee-page {
  max-width: 900px;
  margin: 0 auto;
  font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI',
    sans-serif;
  color: #111;
}

.title {
  font-size: 2.6rem;
  margin-bottom: 1.5rem;
  font-style: italic;
}

.oor-message {
  margin-top: 1rem;
  padding: 0.8rem 1rem;
  background: #ffe6e6;
  border: 1px solid #e57373;
  border-radius: 4px;
  font-size: 0.95rem;
  color: #b71c1c;
  font-weight: 600;
}

.subtitle {
  font-size: 1.2rem;
  margin-bottom: 1rem;
}

.mt {
  margin-top: 2.5rem;
}

.coffee-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  margin-bottom: 2rem;
}

.coffee-row {
  display: grid;
  grid-template-columns: 2fr 0.8fr;
  gap: 1rem;
  align-items: stretch;
}

.coffee-button {
  border: none;
  background: #e5e5e5;
  padding: 0.85rem;
  text-align: left;
  cursor: default;

  display: flex;
  justify-content: space-between;
  align-items: center;

  font-size: 1.1rem;
  font-weight: 500;
}

.coffee-name {
  font-weight: 600;
  margin-bottom: 0.3rem;
}

.coffee-price {
  font-size: 0.95rem;
}

.qty-input {
  border: 1px solid #444;
  text-align: center;
  font-size: 1.4rem;
  padding: 0.4rem 0;
}

.order-box {
  background: #f0f0f0;
  padding: 1.2rem;
  margin-bottom: 2rem;
}

.order-title {
  font-size: 1.2rem;
  margin-bottom: 1rem;
}

.order-inner {
  background: #d9d9d9;
  padding: 1rem;
}

.order-section {
  margin-bottom: 1.2rem;
}

.order-section.total {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.empty-text {
  margin-top: 0.4rem;
  font-size: 0.95rem;
  color: #555;
}

.payment-row {
  display: grid;
  grid-template-columns: 2fr 1.3fr;
  gap: 1.5rem;
  align-items: center;
  margin-top: 1rem;
}

.payment-buttons {
  display: flex;
  gap: 1.2rem;
  flex-wrap: wrap;
}

.payment-button {
  border: none;
  background: #dcdcdc;
  padding: 0.7rem 1.6rem;
  font-size: 1.1rem;
  cursor: pointer;
}

.payment-display {
  height: 55px;
  background: #dcdcdc;
  display: flex;
  align-items: center;
  padding: 0 1rem;
}

.confirm-row {
  margin-top: 2rem;
  display: flex;
  justify-content: flex-end;
}

.confirm-button {
  border: none;
  padding: 0.8rem 1.8rem;
  font-size: 1.1rem;
  background: #222;
  color: white;
  cursor: pointer;
}

.response-message {
  margin-top: 1rem;
  padding: 0.8rem 1rem;
  background: #e6ffe6;
  border: 1px solid #8bc34a;
  border-radius: 4px;
  font-size: 0.95rem;
}
</style>
