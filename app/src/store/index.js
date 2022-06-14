import Vue from 'vue'
import Vuex from 'vuex'
import VuexPersistence from 'vuex-persist'

Vue.use(Vuex)

const vuexLocal = new VuexPersistence({
  key: process.env.VUE_APP_STORE_KEY,
  storage: window.sessionStorage
})

export default new Vuex.Store({
  state: {
    locale: null,
    token: null,
    world: null
  },
  actions: {
    changeWorld({ commit }, world) {
      commit('setWorld', world)
    },
    resetWorld({ commit }) {
      commit('setWorld', null)
    },
    signIn({ commit }, token) {
      commit('setToken', token)
    },
    signOut({ commit }) {
      commit('setToken', null)
      commit('setWorld', null)
    },
    translate({ commit }, locale) {
      commit('setLocale', locale)
    }
  },
  mutations: {
    setLocale(state, locale) {
      state.locale = locale
    },
    setToken(state, token) {
      state.token = token
    },
    setWorld(state, world) {
      state.world = world
    }
  },
  plugins: [vuexLocal.plugin]
})
