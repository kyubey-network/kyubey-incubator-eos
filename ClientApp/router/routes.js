import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import DetailPage from 'components/detail'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'detail', path: '/detail/:id', component: DetailPage, display: 'Detail' },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap' },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Fetch data', icon: 'list' }
]
